namespace Lullaby.ViewModels;

using Groups;

public class GroupViewModel
{
    /// <summary>
    /// The group key
    /// </summary>
    public required string GroupKey { get; init; }

    /// <summary>
    /// The group name
    /// </summary>
    public required string GroupName { get; init; }

    public static GroupViewModel FromGroup(IGroup group) =>
        new() { GroupKey = group.GroupKey, GroupName = group.GroupName };
}
