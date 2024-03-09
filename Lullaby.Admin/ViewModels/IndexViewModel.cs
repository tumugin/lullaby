namespace Lullaby.Admin.ViewModels;

using Services;

public class IndexViewModel
{
    public required IEnumerable<GroupStatistics> GroupStatistics { get; init; }
}
