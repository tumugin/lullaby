namespace Lullaby.Common.Crawler.Scraper.TimeTree;

public interface ITimeTreeApiClient
{
    public Task<TimeTreeApiResult> GetEventsAsync(
        string calendarId,
        DateTimeOffset startDate,
        DateTimeOffset endDate,
        string? pageCursor,
        CancellationToken cancellationToken
    );
}
