namespace Lullaby.Crawler.Events;

public interface IEventDateTime
{
    public DateTimeOffset EventStartDateTimeOffset => this switch
    {
        DetailedEventDateTime detailedEventDateTime => detailedEventDateTime.EventStartDateTime,
        UnDetailedEventDateTime unDetailedEventDateTime => unDetailedEventDateTime.EventStartDate,
        _ => throw new ArgumentException("EventDateTime is not a valid type")
    };

    public DateTimeOffset EventEndDateTimeOffset => this switch
    {
        DetailedEventDateTime detailedEventDateTime => detailedEventDateTime.EventEndDateTime,
        UnDetailedEventDateTime unDetailedEventDateTime => unDetailedEventDateTime.EventEndDate,
        _ => throw new ArgumentException("EventDateTime is not a valid type")
    };
}
