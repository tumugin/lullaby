namespace Lullaby.Crawler.Scraper.Aoseka;

using System.Text.Json;
using AngleSharp;
using AngleSharp.Html.Parser;
using Events;
using RestSharp;
using Utils;

public class AosekaSchedulePageScraper
{
    public const string SchedulePageUrl = "https://gunjonosekai.com/schedule/";

    private RestClient Client { get; }

    public AosekaSchedulePageScraper(RestClient client) => this.Client = client;

    private async Task<string> DownloadDocument(CancellationToken cancellationToken)
    {
        var request = await this.Client.GetAsync(new RestRequest(SchedulePageUrl), cancellationToken);
        return request.Content ?? throw new InvalidDataException("Response must not be null");
    }

    private static async Task<IReadOnlyCollection<AosekaCalenderObject>> ScrapeRawDocument(
        string rawHtml,
        CancellationToken cancellationToken
    )
    {
        var context = BrowsingContext.New(Configuration.Default.WithDefaultLoader());
        var document = await context.OpenAsync(req => req.Content(rawHtml), cancellationToken);
        var element = document.QuerySelector("#eael-event-calendar-2e90714") ??
                      throw new InvalidDataException("calender element was not found");
        var eventsRawJson = element.GetAttribute("data-events") ??
                            throw new InvalidDataException("event attribute was not found");
        var calenderEvents = JsonSerializer.Deserialize<List<AosekaCalenderObject>>(eventsRawJson)
                             ?? throw new InvalidDataException("events cannot be null");
        return calenderEvents.ToArray();
    }

    public async Task<IReadOnlyList<GroupEvent>> ScrapeAsync(CancellationToken cancellationToken)
    {
        var htmlParser = new HtmlParser();
        var downloadedDocument = await this.DownloadDocument(cancellationToken);
        var aosekaEvents = await ScrapeRawDocument(downloadedDocument, cancellationToken);
        var convertedEvents = aosekaEvents
            .Select(v => new GroupEvent
            {
                EventName = v.Title.Let(s =>
                    s.Replace("【LIVE】", "")
                        .Replace("【配信】", "")
                        .Replace("【イベント】", "")
                        .Trim()
                ),
                EventPlace = null,
                EventDateTime = v.ConvertedEventDateTime,
                EventType = v.EventType,
                EventDescription = htmlParser
                    .ParseFragment(v.Description, null!)
                    .Select(x => x.TextContent)
                    .Let(x => string.Join("", x)),
            })
            .ToArray();
        return convertedEvents;
    }
}
