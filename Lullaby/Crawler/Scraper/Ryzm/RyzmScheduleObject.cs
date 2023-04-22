namespace Lullaby.Crawler.Scraper.Ryzm;

using System.Text.Json.Serialization;

public class RyzmScheduleObject
{
    public class RyzmScheduleRootObject
    {
        [JsonPropertyName("props")] public Props Props { get; init; } = null!;
    }

    public class Props
    {
        [JsonPropertyName("pageProps")] public PageProps PageProps { get; init; } = null!;
    }

    public class PageProps
    {
        [JsonPropertyName("data")] public PagePropsData Data { get; init; } = null!;
    }

    public class PagePropsData
    {
        [JsonPropertyName("fetchedData")] public FetchedData FetchedData { get; init; } = null!;
    }

    public class FetchedData
    {
        [JsonPropertyName("live_list")] public LiveList LiveList { get; init; } = null!;
    }

    public class CategoryElement
    {
        [JsonPropertyName("id")] public int Id { get; init; }

        [JsonPropertyName("name")] public string Name { get; init; } = null!;

        [JsonPropertyName("slug")] public string Slug { get; init; } = null!;

        [JsonPropertyName("position")] public int Position { get; init; }
    }

    public class LiveList
    {
        [JsonPropertyName("data")] public IReadOnlyList<LiveListDatum> Data { get; init; } = null!;

        [JsonPropertyName("meta")] public LiveListMeta Meta { get; init; } = null!;
    }

    public class LiveListDatum
    {
        [JsonPropertyName("id")] public Guid Id { get; init; }

        [JsonPropertyName("status")] public string Status { get; init; } = null!;

        [JsonPropertyName("event_date")] public string EventDate { get; init; } = null!;

        [JsonPropertyName("event_date_status")]
        public string EventDateStatus { get; init; } = null!;

        [JsonPropertyName("category")] public CategoryElement Category { get; init; } = null!;

        [JsonPropertyName("venue")] public string Venue { get; init; } = null!;

        [JsonPropertyName("title")] public string Title { get; init; } = null!;

        [JsonPropertyName("artist")] public string Artist { get; init; } = null!;

        [JsonPropertyName("doors_starts_time")]
        public string? DoorsStartsTime { get; init; }

        [JsonPropertyName("price")] public string Price { get; init; } = null!;

        [JsonPropertyName("reservation_setting")]
        public ReservationSetting ReservationSetting { get; init; } = null!;

        [JsonPropertyName("body")] public object Body { get; init; } = null!;

        [JsonPropertyName("publishes_at")] public DateTimeOffset PublishesAt { get; init; }

        [JsonPropertyName("archived")] public int Archived { get; init; }

        [JsonPropertyName("created_at")] public DateTimeOffset CreatedAt { get; init; }

        [JsonPropertyName("updated_at")] public DateTimeOffset UpdatedAt { get; init; }
    }

    public class ReservationSetting
    {
        [JsonPropertyName("ticket_reservation_type")]
        public string TicketReservationType { get; init; } = null!;

        [JsonPropertyName("web_reservation_max_quantity")]
        public object WebReservationMaxQuantity { get; init; } = null!;

        [JsonPropertyName("web_reservation_max_quantity_per_person")]
        public object WebReservationMaxQuantityPerPerson { get; init; } = null!;

        [JsonPropertyName("platforms")] public IReadOnlyList<Platform>? Platforms { get; init; } = null!;
    }

    public class Platform
    {
        [JsonPropertyName("id")] public string Id { get; init; } = null!;

        [JsonPropertyName("url")] public Uri Url { get; init; } = null!;
    }

    public class LiveListMeta
    {
        [JsonPropertyName("current_page")] public int CurrentPage { get; init; }

        [JsonPropertyName("from")] public int From { get; init; }

        [JsonPropertyName("last_page")] public int LastPage { get; init; }

        [JsonPropertyName("links")] public IReadOnlyList<Link> Links { get; init; } = null!;

        [JsonPropertyName("path")] public Uri Path { get; init; } = null!;

        [JsonPropertyName("per_page")] public int PerPage { get; init; }

        [JsonPropertyName("to")] public int To { get; init; }

        [JsonPropertyName("total")] public int Total { get; init; }
    }

    public class Link
    {
        [JsonPropertyName("url")] public Uri Url { get; init; } = null!;

        [JsonPropertyName("label")] public string Label { get; init; } = null!;

        [JsonPropertyName("active")] public bool Active { get; init; }
    }
}
