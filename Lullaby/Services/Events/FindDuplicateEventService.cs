namespace Lullaby.Services.Events;

using Lullaby.Data;
using Lullaby.Models;
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
    public Task<List<Event>> Execute(
        IEnumerable<IFindDuplicateEventService.EventSearchQueryData> eventSearchQueryData
    ) =>
        this.Context
            .Events
            .Where(e =>
                eventSearchQueryData.Select(q => q.EventName).Contains(e.EventName) &&
                eventSearchQueryData.Select(q => q.GroupKey).Contains(e.GroupKey) &&
                eventSearchQueryData.Select(q => q.StartDateTime).Contains(e.EventStarts) &&
                eventSearchQueryData.Select(q => q.EndDateTime).Contains(e.EventEnds)
            )
            .ToListAsync();
}
