namespace Lullaby.Tests.Crawler.Groups;

using Lullaby.Services.Event;
using Microsoft.EntityFrameworkCore;

public class BaseGroupTest : BaseDatabaseTest
{
    private AddEventByGroupEventService AddEventByGroupEventService { get; set; } = null!;
    private FindDuplicateEventService FindDuplicateEventService { get; set; } = null!;
    private UpdateEventByGroupEventService UpdateEventByGroupEventService { get; set; } = null!;

    [SetUp]
    public void Setup()
    {
        this.AddEventByGroupEventService = new AddEventByGroupEventService(this.Context);
        this.FindDuplicateEventService = new FindDuplicateEventService(this.Context);
        this.UpdateEventByGroupEventService = new UpdateEventByGroupEventService(this.Context);
    }

    [Test]
    public async Task TestGetAndUpdateSavedEventsWithEmptyDatabase()
    {
        var testGroup = new TestGroup();
        await testGroup.GetAndUpdateSavedEvents(
            this.AddEventByGroupEventService,
            this.FindDuplicateEventService,
            this.UpdateEventByGroupEventService
        );

        var addedResult = await this.Context.Events.ToListAsync();
        Assert.That(addedResult, Has.Count.EqualTo(2));
    }
}
