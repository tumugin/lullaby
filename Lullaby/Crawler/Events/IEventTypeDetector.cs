namespace Lullaby.Crawler.Events;

using Database.Enums;

public interface IEventTypeDetector
{
    public EventType DetectEventTypeByTitle(string eventTitle);
}
