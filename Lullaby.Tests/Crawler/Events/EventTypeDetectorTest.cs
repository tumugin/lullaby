namespace Lullaby.Tests.Crawler.Events;

using Lullaby.Crawler.Events;

public class EventTypeDetectorTest
{
	[Test, Combinatorial]
	public void TestBattleEventType(
		[Values("MAWA LOOP TOKYO 2022", "MARQUEE祭 Vol.122", "dot yell fes Summer SP")]
		string eventName
	)
	{
		var detector = new EventTypeDetector();
		Assert.That(EventType.BATTLE, Is.EqualTo(detector.DetectEventTypeByTitle(eventName)));
	}
}
