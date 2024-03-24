namespace Lullaby.Common.Crawler.Events;

using Enums;

public interface IEventTypeDetector
{
    public EventType DetectEventTypeByTitle(string eventTitle);
}
