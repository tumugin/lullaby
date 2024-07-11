namespace Lullaby.Common.Crawler.Scraper.Litlink;

using System.Text.Json.Serialization;

public class LitlinkJson
{
    public class Root
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("props")]
        public Props? Props { get; set; }
    }

    public class Props
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("pageProps")]
        public required PageProps PageProps { get; set; }
    }

    public class PageProps
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("profile")]
        public required Profile Profile { get; set; }
    }

    public class Profile
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("profileLinks")]
        public required ProfileLink[] ProfileLinks { get; set; }
    }

    public class ProfileLink
    {
        [JsonPropertyName("buttonLink")] public ButtonLink? ButtonLink { get; set; }
    }

    public class ButtonLink
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("description")] public string? Description { get; set; }

        [JsonPropertyName("url")] public string? Url { get; set; }
    }
}
