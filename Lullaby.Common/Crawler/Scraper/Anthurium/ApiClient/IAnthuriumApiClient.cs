namespace Lullaby.Common.Crawler.Scraper.Anthurium.ApiClient;

public interface IAnthuriumApiClient
{
    public Task<IReadOnlyList<AnthuriumScheduleItem>> GetSchedules(
        DateTimeOffset startDateTime,
        DateTimeOffset endDateTime,
        CancellationToken cancellationToken = default
    );
}
