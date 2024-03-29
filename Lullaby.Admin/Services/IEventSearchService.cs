namespace Lullaby.Admin.Services;

public interface IEventSearchService
{
    public Task<SearchEventResult> SearchEventAsync(
        string? groupKey,
        string? eventName,
        DateTimeOffset? startDateTime,
        DateTimeOffset? endDateTime,
        int page,
        CancellationToken cancellationToken
    );
}
