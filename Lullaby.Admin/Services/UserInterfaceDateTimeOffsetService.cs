namespace Lullaby.Admin.Services;

using System.Globalization;

public class UserInterfaceDateTimeOffsetService(TimeZoneInfo defaultTimeZone) : IUserInterfaceDateTimeOffsetService
{
    public DateTimeOffset ConvertToUserInterfaceDateTimeOffset(DateTimeOffset dateTimeOffset)
        => TimeZoneInfo.ConvertTime(dateTimeOffset, defaultTimeZone);

    public string ConvertToString(DateTimeOffset dateTimeOffset)
        => this.ConvertToUserInterfaceDateTimeOffset(dateTimeOffset)
            .ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);

    public string ConvertToShortString(DateTimeOffset dateTimeOffset) => this
        .ConvertToUserInterfaceDateTimeOffset(dateTimeOffset)
        .ToString("yyyy/MM/dd", CultureInfo.InvariantCulture);

    public DateTimeOffset ConvertFormInputDateTimeToUtcDateTimeOffset(string input)
    {
        var parsedDateTime = DateTime.Parse(input, CultureInfo.InvariantCulture);
        return new DateTimeOffset(parsedDateTime, defaultTimeZone.BaseUtcOffset).ToUniversalTime();
    }
}
