namespace Lullaby.Admin;

using Lullaby.Common.Groups;

public interface IGroupStatisticsService
{
    Task<IEnumerable<GroupStatistics>> GetGroupStatisticsAsync(
        IEnumerable<IGroup> targetGroups,
        CancellationToken cancellationToken
    );
}
