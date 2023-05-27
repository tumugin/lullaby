namespace Lullaby.Groups;

public static class GroupsServiceExtensions
{
    public static IServiceCollection AddGroups(this IServiceCollection serviceCollection) =>
        serviceCollection
            .AddScoped<IGroup, Aoseka>()
            .AddScoped<Aoseka, Aoseka>()
            .AddScoped<IGroup, Kolokol>()
            .AddScoped<Kolokol, Kolokol>()
            .AddScoped<IGroup, Oss>()
            .AddScoped<Oss, Oss>()
            .AddScoped<IGroup, Tebasen>()
            .AddScoped<Tebasen, Tebasen>()
            .AddScoped<IGroup, Yosugala>()
            .AddScoped<Yosugala, Yosugala>()
            .AddScoped<Axelight, Axelight>()
            .AddScoped<IGroup, Axelight>()
            .AddScoped<Prsmin, Prsmin>()
            .AddScoped<IGroup, Prsmin>()
            .AddScoped<IGroupKeys, GroupKeys>();
}
