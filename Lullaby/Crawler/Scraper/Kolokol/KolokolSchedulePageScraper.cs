namespace Lullaby.Crawler.Scraper.Kolokol;

using System.Globalization;
using System.Text.RegularExpressions;
using AngleSharp;
using Events;
using RestSharp;
using Utils;

public partial class KolokolSchedulePageScraper
{
    // サイトのレスポンスがそんなに速くないので、一旦最新1ページ+過去4ページ分だけ決め打ちで取ってくる
    public static readonly IReadOnlyList<string> SchedulePageUrls =
        new[]
        {
            "https://kolokol-official.com/schedule/", "https://kolokol-official.com/schedule/past/",
            "https://kolokol-official.com/schedule/past/num/10",
            "https://kolokol-official.com/schedule/past/num/20", "https://kolokol-official.com/schedule/past/num/30"
        };

    public required RestClient Client { get; init; }

    private async Task<IReadOnlyList<string>> DownloadDocuments(CancellationToken cancellationToken)
    {
        // 全部基本的には同じフォーマットで作られているのでまとめて落としてきてまとめて処理する
        var asyncDocuments = SchedulePageUrls.Select(
            schedulePageUrl => Task.Run(async () =>
            {
                var request = await this.Client.GetAsync(new RestRequest(schedulePageUrl), cancellationToken);
                return request.Content ?? throw new InvalidDataException("Response must not be null");
            })
        );
        return await Task.WhenAll(asyncDocuments);
    }

    [GeneratedRegex("^(\\d+)\\.(\\d+)\\.(\\d+)")]
    private static partial Regex DatePatternRegex();

    [GeneratedRegex("^開場 / (\\d+):(\\d+)")]
    private static partial Regex OpenTimePattenRegex();

    [GeneratedRegex("[\r\n]{2,}")]
    private static partial Regex ManyNewLineRegex();

    [GeneratedRegex("[ ]{2,}")]
    private static partial Regex ManySpaceRegex();

    [GeneratedRegex("^ ", RegexOptions.Multiline)]
    private static partial Regex StartOfLineAndSpace();

    private static async Task<IReadOnlyList<GroupEvent>> ParseDocument(
        string rawHtml,
        CancellationToken cancellationToken
    )
    {
        var eventTypeDetector = new EventTypeDetector();
        var context = BrowsingContext.New(Configuration.Default.WithDefaultLoader());
        var document = await context.OpenAsync(req => req.Content(rawHtml), cancellationToken);
        var scheduleElements = document.QuerySelectorAll(".scdBox");
        return scheduleElements
            .Select(
                scheduleElement =>
                {
                    var dateText = scheduleElement.QuerySelector(".date")?.TextContent;
                    var dateTextRegexMatches = dateText != null ? DatePatternRegex().Matches(dateText) : null;
                    // 一旦日本時間の0時としてパースして作成する
                    var parsedDate = dateTextRegexMatches is { Count: 1 }
                        ? new DateTimeOffset(
                            int.Parse(dateTextRegexMatches[0].Groups[1].Value, CultureInfo.InvariantCulture),
                            int.Parse(dateTextRegexMatches[0].Groups[2].Value, CultureInfo.InvariantCulture),
                            int.Parse(dateTextRegexMatches[0].Groups[3].Value, CultureInfo.InvariantCulture),
                            0,
                            0,
                            0,
                            TimeSpan.FromHours(9)
                        )
                        : throw new InvalidDataException("Date must not be null");

                    // 過去スケジュールと将来のスケジュールでタイトルの入れ方が異なるのでどちらも取ってみる
                    var titleTextOfFutureSchedule = scheduleElement.QuerySelector(".title")?.TextContent;
                    var titleTextOfPastSchedule = scheduleElement
                        .QuerySelectorAll("tr")
                        .FirstOrDefault(e => e.QuerySelector("th")?.TextContent == "TITLE")
                        ?.QuerySelector("td")
                        ?.TextContent;
                    var titleText = (titleTextOfFutureSchedule, titleTextOfPastSchedule) switch
                    {
                        ({ }, { }) => titleTextOfFutureSchedule,
                        ({ }, null) => titleTextOfFutureSchedule,
                        (null, { }) => titleTextOfPastSchedule,
                        _ => throw new InvalidDataException("Title must not be null")
                    };

                    var venueText = scheduleElement.QuerySelector(".place")?.TextContent;

                    var timeText = scheduleElement
                        .QuerySelectorAll("tr")
                        .FirstOrDefault(e => e.QuerySelector("th")?.TextContent == "TIME")
                        ?.QuerySelector("td")
                        ?.TextContent;
                    var openTimeTextRegexMatches = timeText != null ? OpenTimePattenRegex().Matches(timeText) : null;
                    // 開場時間が取れる場合は日本時間としてパースして作成する
                    // FIXME: なぜかC#のコンパイラがvarを使わせてくれないのでここだけ型を指定しておく
                    DateTimeOffset? detailedOpenTime = openTimeTextRegexMatches is { Count: 1 }
                        ? new DateTimeOffset(
                            parsedDate.Year,
                            parsedDate.Month,
                            parsedDate.Day,
                            int.Parse(openTimeTextRegexMatches[0].Groups[1].Value, CultureInfo.InvariantCulture),
                            int.Parse(openTimeTextRegexMatches[0].Groups[2].Value, CultureInfo.InvariantCulture),
                            0,
                            TimeSpan.FromHours(9)
                        )
                        : null;

                    var descriptionText = scheduleElement
                        .QuerySelector("tbody")
                        ?.TextContent
                        .Replace("\t", "")
                        .Let(s => ManySpaceRegex().Replace(s, " "))
                        .Let(s => StartOfLineAndSpace().Replace(s, ""))
                        .Let(s => ManyNewLineRegex().Replace(s, "\n"))
                        .Trim();

                    IEventDateTime eventDateTime = detailedOpenTime switch
                    {
                        { } => new DetailedEventDateTime
                        {
                            EventStartDateTime = detailedOpenTime.Value,
                            // 閉場時間は分からないので一旦開場時間の4時間後にしておく
                            // だいたい入場0.5~1h + ライブ1h~2h + 特典会2hなので4hくらいで十分だと思われる
                            EventEndDateTime = detailedOpenTime.Value.AddHours(4)
                        },
                        _ => new UnDetailedEventDateTime
                        {
                            EventStartDate = parsedDate, EventEndDate = parsedDate.AddDays(1)
                        }
                    };

                    return new GroupEvent
                    {
                        EventName = titleText,
                        EventPlace = venueText,
                        EventDateTime = eventDateTime,
                        EventDescription = descriptionText ?? "",
                        EventType = eventTypeDetector.DetectEventTypeByTitle(titleText)
                    };
                }
            )
            .ToArray();
    }

    public async Task<IReadOnlyList<GroupEvent>> ScrapeAsync(CancellationToken cancellationToken)
    {
        var allDocuments = await this.DownloadDocuments(cancellationToken);
        var allEvents = allDocuments.Select(rawHtml => ParseDocument(rawHtml, cancellationToken));
        return (await Task.WhenAll(allEvents))
            .SelectMany(e => e)
            .ToArray();
    }
}
