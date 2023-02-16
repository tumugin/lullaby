namespace Lullaby.Tests.Crawler.Groups;

using Lullaby.Services.Events;
using Microsoft.EntityFrameworkCore;
using RestSharp;

public class BaseGroupTest : BaseDatabaseTest
{
    private AddEventByGroupEventService AddEventByGroupEventService { get; set; } = null!;
    private FindDuplicateEventService FindDuplicateEventService { get; set; } = null!;
    private UpdateEventByGroupEventService UpdateEventByGroupEventService { get; set; } = null!;
    private RestClient RestClient { get; set; } = null!;

    [SetUp]
    public void Setup()
    {
        this.AddEventByGroupEventService = new AddEventByGroupEventService(this.Context);
        this.FindDuplicateEventService = new FindDuplicateEventService(this.Context);
        this.UpdateEventByGroupEventService = new UpdateEventByGroupEventService(this.Context);
        this.RestClient = new RestClient();
    }

    [Test]
    public async Task TestGetAndUpdateSavedEventsWithEmptyDatabase()
    {
        var testGroup = new TestGroup();
        await testGroup.GetAndUpdateSavedEvents(
            this.AddEventByGroupEventService,
            this.FindDuplicateEventService,
            this.UpdateEventByGroupEventService,
            this.RestClient
        );

        var addedResult = await this.Context.Events.ToListAsync();
        Assert.That(addedResult, Has.Count.EqualTo(2));
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
        await testGroup.GetAndUpdateSavedEvents(
            this.AddEventByGroupEventService,
            this.FindDuplicateEventService,
            this.UpdateEventByGroupEventService,
            this.RestClient
        );
        await testGroup.GetAndUpdateSavedEvents(
            this.AddEventByGroupEventService,
            this.FindDuplicateEventService,
            this.UpdateEventByGroupEventService,
            this.RestClient
        );

        var addedResult = await this.Context.Events.ToListAsync();
        Assert.That(addedResult, Has.Count.EqualTo(2));
        Assert.Multiple(() =>
        {
            Assert.That(addedResult[0].EventDescription, Is.EqualTo("day1 test"));
            Assert.That(addedResult[1].EventDescription, Is.EqualTo("day2 test"));
        });
    }
}
