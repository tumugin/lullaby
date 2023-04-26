namespace Lullaby.ViewModels.Index;

using Groups;

public class IndexViewModel
{
    public required IEnumerable<IGroup> AvailableGroups { get; init; }
}
