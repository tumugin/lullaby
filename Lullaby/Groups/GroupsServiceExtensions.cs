namespace Lullaby.Groups;

public static class GroupsServiceExtensions
{
    public static IServiceCollection AddGroups(this IServiceCollection serviceCollection) =>
        serviceCollection
            .AddScoped<IGroup, Aoseka>()
            .AddScoped<IGroup, Kolokol>()
            .AddScoped<IGroup, Oss>()
            .AddScoped<IGroup, Tebasen>()
            .AddScoped<IGroup, Yosugala>()
            .AddScoped<IGroupKeys, GroupKeys>();
}
