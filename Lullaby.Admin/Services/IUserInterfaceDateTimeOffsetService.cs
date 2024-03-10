namespace Lullaby.Admin.Services;

public interface IUserInterfaceDateTimeOffsetService
{
    public DateTimeOffset ConvertToUserInterfaceDateTimeOffset(DateTimeOffset dateTimeOffset);
    public string ConvertToString(DateTimeOffset dateTimeOffset);
    public string ConvertToShortString(DateTimeOffset dateTimeOffset);
    public DateTimeOffset ConvertFormInputDateTimeToUtcDateTimeOffset(string input);
}
