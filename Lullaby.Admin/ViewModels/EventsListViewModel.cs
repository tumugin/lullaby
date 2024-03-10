namespace Lullaby.Admin.ViewModels;

using Requests.Events;
using Services;

public class EventsListViewModel
{
    public required IndexRequest Request { get; init; }
    public required SearchEventResult Result { get; init; }
}
