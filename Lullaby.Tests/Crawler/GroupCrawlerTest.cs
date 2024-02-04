namespace Lullaby.Tests.Crawler;

using System.Globalization;
using Database.Enums;
using Lullaby.Crawler;
using Lullaby.Crawler.Events;
using Lullaby.Crawler.Scraper;
using Lullaby.Db;
using Groups;
using Microsoft.EntityFrameworkCore;

public class GroupCrawlerTest : BaseDatabaseTest
{
    private AddEventByGroupEventService addEventByGroupEventService = null!;
    private FindDuplicateEventService findDuplicateEventService = null!;
    private UpdateEventByGroupEventService updateEventByGroupEventService = null!;

    [SetUp]
    public void Setup()
    {
        this.addEventByGroupEventService = new AddEventByGroupEventService(this.Context);
        this.findDuplicateEventService = new FindDuplicateEventService(this.Context);
        this.updateEventByGroupEventService = new UpdateEventByGroupEventService(this.Context);
    }

    [Test]
    public async Task TestGetAndUpdateSavedEventsWithEmptyDatabase()
    {
        var testGroup = new TestGroup();
        var scraper = new TestGroupPageScraper();
        var groupCrawler = new GroupCrawler(
            this.addEventByGroupEventService,
            this.findDuplicateEventService,
            this.updateEventByGroupEventService,
            new[] { scraper },
            this.Context
        );
        await groupCrawler.GetAndUpdateSavedEvents(testGroup, default);

        var addedResult = await this.Context.Events.ToArrayAsync();
        Assert.That(addedResult, Has.Length.EqualTo(2));
        Assert.Multiple(() =>
        {
            Assert.That(addedResult[0].EventDescription, Is.EqualTo("day1 test"));
            Assert.That(addedResult[1].EventDescription, Is.EqualTo("day2 test"));
        });
    }

    [Test]
    public async Task TestGetAndUpdateSavedEventsWithDuplicate()
    {
        var testGroup = new TestGroup();
        var scraper = new TestGroupPageScraper();
        var groupCrawler = new GroupCrawler(
            this.addEventByGroupEventService,
            this.findDuplicateEventService,
            this.updateEventByGroupEventService,
            new[] { scraper },
            this.Context
        );

        // run it twice
        await groupCrawler.GetAndUpdateSavedEvents(testGroup, default);
        await groupCrawler.GetAndUpdateSavedEvents(testGroup, default);

        var addedResult = await this.Context.Events.ToListAsync();
        Assert.That(addedResult, Has.Count.EqualTo(2));
        Assert.Multiple(() =>
        {
            Assert.That(addedResult[0].EventDescription, Is.EqualTo("day1 test"));
            Assert.That(addedResult[1].EventDescription, Is.EqualTo("day2 test"));
        });
    }

    private sealed class TestGroup : IGroup
    {
        public string GroupKey => "test";
        public string GroupName => "テスト";
    }

    private sealed class TestGroupPageScraper : ISchedulePageScraper
    {
        public Type TargetGroup => typeof(TestGroup);

        public Task<IReadOnlyList<GroupEvent>> ScrapeAsync(CancellationToken cancellationToken) =>
            Task.FromResult<IReadOnlyList<GroupEvent>>(new GroupEvent[]
            {
                new()
                {
                    EventName = "TOKYO IDOL FESTIVAL 2022 supported by にしたんクリニック",
                    EventPlace = "お台場・青海周辺エリア",
                    EventType = EventType.Fes,
                    EventDescription = "day1 test",
                    EventDateTime = new DetailedEventDateTime
                    {
                        EventStartDateTime = DateTimeOffset.Parse(
                            "2022-08-05 10:00:00+09:00",
                            CultureInfo.InvariantCulture
                        ),
                        EventEndDateTime = DateTimeOffset.Parse(
                            "2022-08-05 20:00:00+09:00",
                            CultureInfo.InvariantCulture
                        )
                    }
                },
                new()
                {
                    EventName = "TOKYO IDOL FESTIVAL 2022 supported by にしたんクリニック",
                    EventPlace = "お台場・青海周辺エリア",
                    EventType = EventType.Fes,
                    EventDescription = "day2 test",
                    EventDateTime = new DetailedEventDateTime
                    {
                        EventStartDateTime = DateTimeOffset.Parse(
                            "2022-08-06 10:00:00+09:00",
                            CultureInfo.InvariantCulture
                        ),
                        EventEndDateTime = DateTimeOffset.Parse(
                            "2022-08-06 20:00:00+09:00",
                            CultureInfo.InvariantCulture
                        )
                    }
                }
            });
    }
}
