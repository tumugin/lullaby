namespace Lullaby;

using Services.Event;

public static class DiConfig
{
    public static void BuildDi(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<GetEventsByGroupKeyService, GetEventsByGroupKeyService>();
    }
}
