namespace Lullaby.Crawler.Events;

using Common.Enums;

public interface IEventTypeDetector
{
    public EventType DetectEventTypeByTitle(string eventTitle);
}
