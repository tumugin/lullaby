namespace Lullaby.Responses;

using Microsoft.AspNetCore.Mvc;

public class HandledErrorResponse : ProblemDetails
{
    public ApiErrorTypes ApiErrorType { get; }

    public HandledErrorResponse(ApiErrorTypes apiErrorType)
    {
        this.ApiErrorType = apiErrorType;
        this.Title = this.ApiErrorType switch
        {
            ApiErrorTypes.GroupNotFound => "Group was not found",
            _ => "Unknown error occured"
        };
    }
}
