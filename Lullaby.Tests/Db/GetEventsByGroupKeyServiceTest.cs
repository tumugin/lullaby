namespace Lullaby.Tests.Db;

using System.Globalization;
using Lullaby.Db;
using Seeder;

public class GetEventsByGroupKeyServiceTest : BaseDatabaseTest
{
    private GetEventsByGroupKeyService GetEventsByGroupKeyService { get; set; } = null!;
    private EventSeeder EventSeeder { get; set; } = null!;

    [SetUp]
    public void Setup()
    {
        this.GetEventsByGroupKeyService =
            new GetEventsByGroupKeyService(this.Context);
        this.EventSeeder = new EventSeeder(this.Context);
    }

    [Test]
    public async Task TestExecuteWithRange()
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

        var results = await this.GetEventsByGroupKeyService.Execute(
            expectedEvent.GroupKey,
            new[] { expectedEvent.EventType },
            DateTimeOffset.Parse(
                "2022-11-01 00:00:00+09:00",
                CultureInfo.InvariantCulture
            ),
            DateTimeOffset.Parse(
                "2022-11-30 23:59:59+09:00",
                CultureInfo.InvariantCulture
            ),
            default
        );

        Assert.Multiple(() =>
        {
            Assert.That(results, Has.Count.EqualTo(1));
            Assert.That(results[0], Is.EqualTo(expectedEvent));
        });
    }

    [Test]
    public async Task TestExecute()
    {
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

        var results = await this.GetEventsByGroupKeyService.Execute(
            expectedEvent.GroupKey,
            new[] { expectedEvent.EventType },
            default
        );

        Assert.That(results[0], Is.EqualTo(expectedEvent));
    }
}
