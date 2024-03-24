namespace Lullaby.Jobs.Db;

using Common.Groups;
using Database.DbContext;
using Microsoft.EntityFrameworkCore;
using Services.Crawler.Events;

public class CleanupOldScheduledEventsService(LullabyContext context) : ICleanupOldScheduledEventsService
{
    private readonly IReadOnlyCollection<string> searchWords = new[] { "予定", "仮" }.AsReadOnly();

    public async Task ExecuteAsync(IGroup targetGroup, CancellationToken cancellationToken)
    {
        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

        var scheduledEvents = await context.Events
            .Where(v => v.GroupKey == targetGroup.GroupKey)
            .Where(v => this.searchWords.Select(x => v.EventName.Contains(x)).Any())
            .ToArrayAsync(cancellationToken);

        var startDate = scheduledEvents.Min(x => x.EventStarts);
        var endDate = scheduledEvents.Max(x => x.EventEnds);

        var eventsToSearch = await context.Events
            .Where(v => v.GroupKey == targetGroup.GroupKey)
            .Where(v => !this.searchWords.Select(x => v.EventName.Contains(x)).Any())
            .Where(v => v.EventStarts >= startDate)
            .Where(v => v.EventEnds <= endDate)
            .ToArrayAsync(cancellationToken);

        // 仮予定イベントの開始時刻から終了時刻までの間に正式なイベントが存在する仮予定イベントを検索する
        var overlappingEvents = scheduledEvents
            .Where(v => eventsToSearch
                .Where(x => x.EventStarts >= v.EventStarts)
                .Any(x => x.EventEnds <= v.EventEnds)
            )
            .ToArray();

        context.Events.RemoveRange(overlappingEvents);
        await context.SaveChangesAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);
    }
}
