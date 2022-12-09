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
        public required string groupKey { get; init; }
        public required string eventName { get; init; }
        public required DateTimeOffset startDateTime { get; init; }
        public required DateTimeOffset endDateTime { get; init; }
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
                eventSearchQueryData.Select(q => q.eventName).Contains(e.EventName) &&
                eventSearchQueryData.Select(q => q.groupKey).Contains(e.GroupKey) &&
                eventSearchQueryData.Select(q => q.startDateTime).Contains(e.EventStarts) &&
                eventSearchQueryData.Select(q => q.endDateTime).Contains(e.EventEnds)
            )
            .ToListAsync();
}
