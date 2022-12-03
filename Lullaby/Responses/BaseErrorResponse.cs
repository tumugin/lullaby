namespace Lullaby.Responses;

public class BaseErrorResponse
{
    public bool IsError => true;
    public ApiErrorTypes ErrorType { get; set; } = ApiErrorTypes.Unknown;
}
