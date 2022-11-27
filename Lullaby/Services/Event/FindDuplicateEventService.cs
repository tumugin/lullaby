namespace Lullaby.Services.Event;

using Data;
using Microsoft.EntityFrameworkCore;
using Models;

public class FindDuplicateEventService
{
    private LullabyContext Context { get; }

    public FindDuplicateEventService(LullabyContext context) => Context = context;

    /**
     * 既にDBに保存された重複するイベントを検索する
     * サイト上には基本的にIDはないので、イベント名と開始終了時刻での検索を行う
     * (タイトルのみだとTOKYO IDOL FESTIVALのような複数日出演するイベントで壊れる)
     */
    public Task<Event?> Execute(
        string groupKey, string eventName, DateTimeOffset startDateTime, DateTimeOffset endDateTime
    ) =>
        Context
            .Events
            .Where(e =>
                e.EventName == eventName &&
                e.GroupKey == groupKey &&
                e.EventStarts == startDateTime &&
                e.EventEnds == endDateTime
            )
            .FirstOrDefaultAsync();
}
