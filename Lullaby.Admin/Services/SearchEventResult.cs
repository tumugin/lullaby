namespace Lullaby.Admin.Services;

using Database.Models;

public class SearchEventResult
{
    public required IList<Event> Events { get; init; }
    public required int Limit { get; init; }
    public required int CurrentPage { get; init; }
    public required int TotalEvents { get; init; }

    public int TotalPages => (int)Math.Ceiling((double)this.TotalEvents / this.Limit);
}
