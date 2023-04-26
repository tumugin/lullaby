namespace Lullaby.Crawler.Scraper.Aoseka;

using System.Text.Json;
using AngleSharp;
using AngleSharp.Html.Parser;
using Events;
using Groups;
using RestSharp;
using Utils;

public class AosekaSchedulePageScraper : ISchedulePageScraper
{
    public const string SchedulePageUrl = "https://gunjonosekai.com/schedule/";
    private readonly IBrowsingContext browsingContext;

    private readonly RestClient client;

    public AosekaSchedulePageScraper(
        RestClient client,
        IBrowsingContext browsingContext
    )
    {
        this.client = client;
        this.browsingContext = browsingContext;
    }

    public Type TargetGroup => typeof(Aoseka);

    public async Task<IReadOnlyList<GroupEvent>> ScrapeAsync(CancellationToken cancellationToken)
    {
        var htmlParser = new HtmlParser();
        var downloadedDocument = await this.DownloadDocument(cancellationToken);
        var aosekaEvents = await this.ScrapeRawDocument(downloadedDocument, cancellationToken);
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
                    .Let(x => string.Join("", x))
            })
            .ToArray();
        return convertedEvents;
    }

    private async Task<string> DownloadDocument(CancellationToken cancellationToken)
    {
        var request = await this.client.GetAsync(new RestRequest(SchedulePageUrl), cancellationToken);
        return request.Content ?? throw new InvalidDataException("Response must not be null");
    }

    private async Task<IReadOnlyCollection<AosekaCalenderObject>> ScrapeRawDocument(
        string rawHtml,
        CancellationToken cancellationToken
    )
    {
        var document = await this.browsingContext.OpenAsync(req => req.Content(rawHtml), cancellationToken);
        var element = document.QuerySelector("#eael-event-calendar-2e90714") ??
                      throw new InvalidDataException("calender element was not found");
        var eventsRawJson = element.GetAttribute("data-events") ??
                            throw new InvalidDataException("event attribute was not found");
        var calenderEvents = JsonSerializer.Deserialize<List<AosekaCalenderObject>>(eventsRawJson)
                             ?? throw new InvalidDataException("events cannot be null");
        return calenderEvents.ToArray();
    }
}
