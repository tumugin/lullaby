namespace Lullaby.Admin.Services;

using Common.Groups;

public class GroupStatistics
{
    public required IGroup Group { get; init; }
    public required int TotalEvents { get; init; }
    public required DateTimeOffset LatestEventDate { get; init; }
    public required DateTimeOffset LastUpdatedAt { get; init; }
}
