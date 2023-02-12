namespace Lullaby.Crawler.Scraper.Aoseka;

using System.Collections.ObjectModel;
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

    private async Task<string> DownloadDocument()
    {
        var request = await this.Client.GetAsync(new RestRequest(SchedulePageUrl));
        return request.Content ?? throw new InvalidDataException("Response must not be null");
    }

    private async Task<ReadOnlyCollection<AosekaCalenderObject>> ScrapeRawDocument(string rawHtml)
    {
        var context = BrowsingContext.New(Configuration.Default.WithDefaultLoader());
        var document = await context.OpenAsync(req => req.Content(rawHtml));
        var element = document.QuerySelector("#eael-event-calendar-2e90714") ??
                      throw new InvalidDataException("calender element was not found");
        var eventsRawJson = element.GetAttribute("data-events") ??
                            throw new InvalidDataException("event attribute was not found");
        var calenderEvents = JsonSerializer.Deserialize<List<AosekaCalenderObject>>(eventsRawJson)
                             ?? throw new InvalidDataException("events cannot be null");
        return calenderEvents.AsReadOnly();
    }

    public async Task<IEnumerable<GroupEvent>> ScrapeAsync()
    {
        var htmlParser = new HtmlParser();
        var downloadedDocument = await this.DownloadDocument();
        var aosekaEvents = await this.ScrapeRawDocument(downloadedDocument);
        var convertedEvents = aosekaEvents.Select(v => new GroupEvent
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
                .ParseFragment(v.Description, null)
                .Select(x => x.TextContent)
                .Let(x => string.Join("", x)),
        });
        return convertedEvents;
    }
}
