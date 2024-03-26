namespace Lullaby.Tests.Db;

using System.Globalization;
using Common.Enums;
using Common.Groups;
using Database.Models;
using Jobs.Db;
using Microsoft.EntityFrameworkCore;

public class CleanupOldScheduledEventsServiceTest : BaseDatabaseTest
{
    private CleanupOldScheduledEventsService cleanupOldScheduledEventsService = null!;

    [SetUp]
    public void Setup() => this.cleanupOldScheduledEventsService = new CleanupOldScheduledEventsService(this.Context);

    private async Task Seed()
    {
        await this.Context.Events.AddRangeAsync([
            // 2024-03-13
            // 削除されないイベント
            new Event
            {
                Id = 1,
                GroupKey = "tenrin",
                EventName = "YOLOZ DYNAMITE Vol.1",
                EventDescription = "OPEN 17:00 / START 17:30",
                EventStarts = DateTimeOffset.Parse("2024-03-13T00:00:00+09:00", CultureInfo.InvariantCulture),
                EventEnds = DateTimeOffset.Parse("2024-03-14T00:00:00+09:00", CultureInfo.InvariantCulture),
                IsDateTimeDetailed = false,
                EventPlace = null,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow,
                EventType = EventType.Battle
            },
            // 2024-03-14
            // 削除されるイベント
            new Event
            {
                Id = 2,
                GroupKey = "tenrin",
                EventName = "ライブ（仮）",
                EventDescription = "",
                EventStarts = DateTimeOffset.Parse("2024-03-14T00:00:00+09:00", CultureInfo.InvariantCulture),
                EventEnds = DateTimeOffset.Parse("2024-03-15T00:00:00+09:00", CultureInfo.InvariantCulture),
                IsDateTimeDetailed = false,
                EventPlace = null,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow,
                EventType = EventType.Battle
            },
            // 削除されないイベント
            new Event
            {
                Id = 3,
                GroupKey = "tenrin",
                EventName = "HEROINES WHITEDAY",
                EventDescription = "『HEROINES WHITE DAY』に出演します",
                EventStarts = DateTimeOffset.Parse("2024-03-14T00:00:00+09:00", CultureInfo.InvariantCulture),
                EventEnds = DateTimeOffset.Parse("2024-03-15T00:00:00+09:00", CultureInfo.InvariantCulture),
                IsDateTimeDetailed = false,
                EventPlace = null,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow,
                EventType = EventType.Battle
            },
            // 2024-03-15
            // 削除されないイベント
            new Event
            {
                Id = 4,
                GroupKey = "tenrin",
                EventName = "ライブ（仮）",
                EventDescription = "",
                EventStarts = DateTimeOffset.Parse("2024-03-15T00:00:00+09:00", CultureInfo.InvariantCulture),
                EventEnds = DateTimeOffset.Parse("2024-03-16T00:00:00+09:00", CultureInfo.InvariantCulture),
                IsDateTimeDetailed = false,
                EventPlace = null,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow,
                EventType = EventType.Battle
            }
        ]);
        await this.Context.SaveChangesAsync();
    }

    [Test]
    public async Task TestExecute()
    {
        await this.Seed();
        await this.cleanupOldScheduledEventsService.ExecuteAsync(
            new Tenrin(),
            default
        );

        var events = await this.Context.Events.ToArrayAsync();
        Assert.Multiple(() =>
        {
            Assert.That(events, Has.Length.EqualTo(3));
            Assert.That(events[0].Id, Is.EqualTo(1));
            Assert.That(events[1].Id, Is.EqualTo(3));
            Assert.That(events[2].Id, Is.EqualTo(4));
        });
    }
}
