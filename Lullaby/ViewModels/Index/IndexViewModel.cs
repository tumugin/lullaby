namespace Lullaby.ViewModels.Index;

using Common.Groups;

public class IndexViewModel
{
    public required IEnumerable<IGroup> AvailableGroups { get; init; }
}
