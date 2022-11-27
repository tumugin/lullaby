namespace Lullaby.Crawler.Scraper;

using System.Text.Json;
using AngleSharp;
using Events;
using RestSharp;

public class AosekaSchedulePageScraper
{
    public const string SchedulePageUrl = "https://gunjonosekai.com/schedule/";

    private RestClient Client { get; }

    public AosekaSchedulePageScraper(RestClient client) => Client = client;

    private async Task<string> DownloadDocument()
    {
        var request = await Client.GetAsync(new RestRequest(SchedulePageUrl));
        return request.Content ?? throw new InvalidDataException("Response must not be null");
    }

    private async Task<List<AosekaCalenderObject>> ScrapeRawDocument(string rawHtml)
    {
        var context = BrowsingContext.New(Configuration.Default.WithDefaultLoader());
        var document = await context.OpenAsync(req => req.Content(rawHtml));
        var element = document.QuerySelector("#eael-event-calendar-2e90714") ??
                      throw new InvalidDataException("calender element was not found");
        var eventsRawJson = element.GetAttribute("data-events") ??
                            throw new InvalidDataException("event attribute was not found");
        var calenderEvents = JsonSerializer.Deserialize<List<AosekaCalenderObject>>(eventsRawJson)
                             ?? throw new InvalidDataException("events cannot be null");
        return calenderEvents;
    }

    public async Task<GroupEvent[]> ScrapeAsync()
    {
        var downloadedDocument = await DownloadDocument();
        var aosekaEvents = await ScrapeRawDocument(downloadedDocument);
        aosekaEvents.Select(v => new GroupEvent
        {
            EventName = v.Title,
            EventPlace = null,
            EventDateTime = v.ConvertedEventDateTime,
            EventType = v.EventType,
            EventDescription = v.Description,
        });
        throw new NotImplementedException();
    }
}
