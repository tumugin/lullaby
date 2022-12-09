namespace Lullaby.Services.Event;

using Data;
using Microsoft.EntityFrameworkCore;
using Models;

public class FindDuplicateEventService
{
    private LullabyContext Context { get; }

    public FindDuplicateEventService(LullabyContext context) => this.Context = context;

    public class EventSearchQueryData
    {
        public required string GroupKey { get; init; }
        public required string EventName { get; init; }
        public required DateTimeOffset StartDateTime { get; init; }
        public required DateTimeOffset EndDateTime { get; init; }
    }

    /**
     * 既にDBに保存された重複するイベントを検索する
     * サイト上には基本的にIDはないので、イベント名と開始終了時刻での検索を行う
     * (タイトルのみだとTOKYO IDOL FESTIVALのような複数日出演するイベントで壊れる)
     */
    public Task<List<Event>> Execute(
        List<EventSearchQueryData> eventSearchQueryData
    ) =>
        Context
            .Events
            .Where(e =>
                eventSearchQueryData.Select(q => q.EventName).Contains(e.EventName) &&
                eventSearchQueryData.Select(q => q.GroupKey).Contains(e.GroupKey) &&
                eventSearchQueryData.Select(q => q.StartDateTime).Contains(e.EventStarts) &&
                eventSearchQueryData.Select(q => q.EndDateTime).Contains(e.EventEnds)
            )
            .ToListAsync();
}
