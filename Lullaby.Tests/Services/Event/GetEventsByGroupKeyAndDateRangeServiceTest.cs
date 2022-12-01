namespace Lullaby.Tests.Services.Event;

using System.Globalization;
using Lullaby.Services.Event;
using Seeder;

public class GetEventsByGroupKeyAndDateRangeServiceTest : BaseDatabaseTest
{
    private GetEventsByGroupKeyAndDateRangeService GetEventsByGroupKeyAndDateRangeService { get; set; } = null!;
    private EventSeeder EventSeeder { get; set; } = null!;

    [SetUp]
    public void Setup()
    {
        this.GetEventsByGroupKeyAndDateRangeService =
            new GetEventsByGroupKeyAndDateRangeService(this.Context);
        this.EventSeeder = new EventSeeder(this.Context);
    }

    [Test]
    public async Task TestExecute()
    {
        await this.EventSeeder.SeedEvent(
            eventStarts: DateTimeOffset.Parse(
                "2022-10-15 19:30:00+09:00",
                CultureInfo.InvariantCulture
            ),
            eventEnds: DateTimeOffset.Parse(
                "2022-10-15 21:30:00+09:00",
                CultureInfo.InvariantCulture
            )
        );
        await this.EventSeeder.SeedEvent(
            eventStarts: DateTimeOffset.Parse(
                "2022-12-15 19:30:00+09:00",
                CultureInfo.InvariantCulture
            ),
            eventEnds: DateTimeOffset.Parse(
                "2022-12-15 21:30:00+09:00",
                CultureInfo.InvariantCulture
            )
        );
        var expectedEvent = await this.EventSeeder.SeedEvent(
            eventStarts: DateTimeOffset.Parse(
                "2022-11-30 19:30:00+09:00",
                CultureInfo.InvariantCulture
            ),
            eventEnds: DateTimeOffset.Parse(
                "2022-12-01 00:30:00+09:00",
                CultureInfo.InvariantCulture
            )
        );

        var results = await this.GetEventsByGroupKeyAndDateRangeService.Execute(
            expectedEvent.GroupKey,
            new[] { expectedEvent.EventType },
            DateTimeOffset.Parse(
                "2022-11-01 00:00:00+09:00",
                CultureInfo.InvariantCulture
            ),
            DateTimeOffset.Parse(
                "2022-11-30 23:59:59+09:00",
                CultureInfo.InvariantCulture
            )
        );

        Assert.Multiple(() =>
        {
            Assert.That(results.Count(), Is.EqualTo(1));
            Assert.That(results.FirstOrDefault(), Is.EqualTo(expectedEvent));
        });
    }
}
