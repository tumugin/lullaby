namespace Lullaby.Admin.Services;

using Common.Groups;

public interface IGroupStatisticsService
{
    Task<IEnumerable<GroupStatistics>> GetGroupStatisticsAsync(
        IEnumerable<IGroup> targetGroups,
        CancellationToken cancellationToken
    );
}
