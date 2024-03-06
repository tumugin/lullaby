namespace Lullaby.Admin;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lullaby.Common.Groups;
using Lullaby.Database.DbContext;
using Microsoft.EntityFrameworkCore;

public class GroupStatisticsService(LullabyContext lullabyContext) : IGroupStatisticsService
{
    public async Task<IEnumerable<GroupStatistics>> GetGroupStatisticsAsync(
        IEnumerable<IGroup> targetGroups,
        CancellationToken cancellationToken
    )
    {
        var targetGroupKeys = targetGroups.Select(g => g.GroupKey).ToArray();
        var result = await lullabyContext.Events
            .Where(e => targetGroupKeys.Contains(e.GroupKey))
            .GroupBy(e => e.GroupKey)
            .Select(g => new
            {
                GroupKey = g.Key,
                TotalEvents = g.Count(),
                LatestEventDate = g.Max(e => e.EventStarts),
                LastUpdatedAt = g.Max(e => e.UpdatedAt)
            })
            .ToArrayAsync(cancellationToken);
        return result.Select(r => new GroupStatistics
        {
            Group = targetGroups.Single(g => g.GroupKey == r.GroupKey),
            TotalEvents = r.TotalEvents,
            LatestEventDate = r.LatestEventDate,
            LastUpdatedAt = r.LastUpdatedAt
        });
    }
}
