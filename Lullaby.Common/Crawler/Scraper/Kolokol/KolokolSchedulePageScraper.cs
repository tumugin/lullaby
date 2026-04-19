namespace Lullaby.Common.Crawler.Scraper.Kolokol;

using System.Globalization;
using System.Text.RegularExpressions;
using AngleSharp;
using Events;
using Groups;
using RestSharp;
using Utils;

public partial class KolokolSchedulePageScraper : ISchedulePageScraper
{
    // サイトのレスポンスがそんなに速くないので、一旦最新1ページ+過去4ページ分だけ決め打ちで取ってくる
    public static readonly IReadOnlyList<string> SchedulePageUrls =
        new[]
        {
            "https://kolokol-official.com/schedule/", "https://kolokol-official.com/schedule/past/",
            "https://kolokol-official.com/schedule/past/num/10",
            "https://kolokol-official.com/schedule/past/num/20", "https://kolokol-official.com/schedule/past/num/30"
        };

    private static readonly string RequestUserAgent =
        "Mozilla/5.0 (iPhone; CPU iPhone OS 14_6 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/14.0.3 Mobile/15E148 Safari/604.1";

    private readonly IBrowsingContext browsingContext;

    private readonly RestClient client;
    private readonly IEventTypeDetector eventTypeDetector;

    public KolokolSchedulePageScraper(RestClient client, IBrowsingContext browsingContext,
        IEventTypeDetector eventTypeDetector)
    {
        this.client = client;
        this.browsingContext = browsingContext;
        this.eventTypeDetector = eventTypeDetector;
    }

    public Type TargetGroup => typeof(Kolokol);

    public async Task<IReadOnlyList<GroupEvent>> ScrapeAsync(CancellationToken cancellationToken)
    {
        var allDocuments = await this.DownloadDocuments(cancellationToken);
        var allEvents = allDocuments.Select(rawHtml => this.ParseDocument(rawHtml, cancellationToken));
        return (await Task.WhenAll(allEvents))
            .SelectMany(e => e)
            .ToArray();
    }

    private async Task<IReadOnlyList<string>> DownloadDocuments(CancellationToken cancellationToken)
    {
        var scheduleDocuments = new List<string>();
        foreach (var url in SchedulePageUrls)
        {
            var request = new RestRequest(url);
            request.AddHeader("User-Agent", RequestUserAgent);
            var response = await this.client.GetAsync(request, cancellationToken);
            var document = response.Content ?? throw new InvalidDataException("Response must not be null");
            scheduleDocuments.Add(document);
        }

        return scheduleDocuments;
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

    private async Task<IReadOnlyList<GroupEvent>> ParseDocument(
        string rawHtml,
        CancellationToken cancellationToken
    )
    {
        using var document = await this.browsingContext.OpenAsync(req => req.Content(rawHtml), cancellationToken);
        var scheduleElements = document.QuerySelectorAll(".scdBox");
        return scheduleElements
            .Select(scheduleElement =>
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
                        (not null, not null) => titleTextOfFutureSchedule,
                        (not null, null) => titleTextOfFutureSchedule,
                        (null, not null) => titleTextOfPastSchedule,
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
                        not null => new DetailedEventDateTime
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
                        EventType = this.eventTypeDetector.DetectEventTypeByTitle(titleText)
                    };
                }
            )
            .ToArray();
    }
}
