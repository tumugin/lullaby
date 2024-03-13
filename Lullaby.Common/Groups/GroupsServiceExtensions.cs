namespace Lullaby.Common.Groups;

using Microsoft.Extensions.DependencyInjection;

public static class GroupsServiceExtensions
{
    public static IServiceCollection AddGroups(this IServiceCollection serviceCollection) =>
        serviceCollection
            .AddScoped<IGroupKeys, GroupKeys>()
            // groups
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
            .AddScoped<IGroup, Tenhana>()
            .AddScoped<Tenhana, Tenhana>()
            .AddScoped<IGroup, Tenrin>()
            .AddScoped<Tenrin, Tenrin>()
            .AddScoped<IGroup, Anthurium>()
            .AddScoped<Anthurium, Anthurium>()
            .AddScoped<Narlow, Narlow>()
            .AddScoped<IGroup, Narlow>()
            .AddScoped<Yoloz, Yoloz>()
            .AddScoped<IGroup, Yoloz>();
}
