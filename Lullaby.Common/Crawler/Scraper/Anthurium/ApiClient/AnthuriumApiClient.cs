namespace Lullaby.Common.Crawler.Scraper.Anthurium.ApiClient;

using System.Net.Http.Json;
using System.Text.Json;
using Flurl;

public class AnthuriumApiClient : IAnthuriumApiClient
{
    private readonly HttpClient httpClient;
    private readonly string baseUrl;

    public AnthuriumApiClient(HttpClient httpClient, string baseUrl)
    {
        this.httpClient = httpClient;
        this.baseUrl = baseUrl;
    }

    public async Task<IReadOnlyList<AnthuriumScheduleItem>> GetSchedules(
        DateTimeOffset startDateTime,
        DateTimeOffset endDateTime,
        CancellationToken cancellationToken
    )
    {
        var requestUri = this.baseUrl
            .AppendPathSegments("schedule", "events")
            .SetQueryParams(new
            {
                start = startDateTime.ToUnixTimeMilliseconds(), end = endDateTime.ToUnixTimeMilliseconds()
            })
            .ToUri();

        var result = await this.httpClient.GetFromJsonAsync<AnthuriumScheduleItem[]>(requestUri, cancellationToken);

        if (result == null)
        {
            throw new JsonException("The result must not be null.");
        }

        return result;
    }
}
