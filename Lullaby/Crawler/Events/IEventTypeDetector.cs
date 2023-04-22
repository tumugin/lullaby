namespace Lullaby.Crawler.Events;

public interface IEventTypeDetector
{
    public EventType DetectEventTypeByTitle(string eventTitle);
}
