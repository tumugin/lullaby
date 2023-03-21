namespace Lullaby.Services.Events;

using Data;
using Models;
using Microsoft.EntityFrameworkCore;

public class FindDuplicateEventService : IFindDuplicateEventService
{
    private LullabyContext Context { get; }

    public FindDuplicateEventService(LullabyContext context) => this.Context = context;

    /**
     * 既にDBに保存された重複するイベントを検索する
     * サイト上には基本的にIDはないので、イベント名と開始終了時刻での検索を行う
     * (タイトルのみだとTOKYO IDOL FESTIVALのような複数日出演するイベントで壊れる)
     */
    public async Task<IReadOnlyList<Event>> Execute(
        IEnumerable<IFindDuplicateEventService.EventSearchQueryData> eventSearchQueryData,
        CancellationToken cancellationToken)
    {
        var query = eventSearchQueryData.ToArray();
        return await this.Context
            .Events
            .Where(e =>
                query.Select(q => q.EventName).Contains(e.EventName) &&
                query.Select(q => q.GroupKey).Contains(e.GroupKey) &&
                query.Select(q => q.StartDateTime).Contains(e.EventStarts) &&
                query.Select(q => q.EndDateTime).Contains(e.EventEnds)
            )
            .ToArrayAsync(cancellationToken);
    }
}
