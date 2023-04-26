namespace Lullaby.Groups;

public class GroupKeys : IGroupKeys
{
    private readonly IEnumerable<IGroup> baseGroups;

    public GroupKeys(IEnumerable<IGroup> baseGroups) => this.baseGroups = baseGroups;

    public IGroup? GetGroupByKey(string groupKey) =>
        this.baseGroups.FirstOrDefault(v => v.GroupKey == groupKey);
}
