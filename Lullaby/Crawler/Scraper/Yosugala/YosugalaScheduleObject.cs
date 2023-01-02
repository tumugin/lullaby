namespace Lullaby.Crawler.Scraper.Yosugala;

using System.Text.Json.Serialization;

public class YosugalaScheduleObject
{
    public class YosugalaSchedulePageRootObject
    {
        [JsonPropertyName("props")] public Props Props { get; set; } = null!;
    }

    public class Props
    {
        [JsonPropertyName("pageProps")] public PageProps PageProps { get; set; } = null!;
    }

    public class PageProps
    {
        [JsonPropertyName("data")] public PagePropsData Data { get; set; } = null!;
    }

    public class PagePropsData
    {
        [JsonPropertyName("fetchedData")] public FetchedData FetchedData { get; set; } = null!;
    }

    public class FetchedData
    {
        [JsonPropertyName("live_list")] public LiveList LiveList { get; set; } = null!;
    }

    public class CategoryElement
    {
        [JsonPropertyName("id")] public int Id { get; set; }

        [JsonPropertyName("name")] public string Name { get; set; } = null!;

        [JsonPropertyName("slug")] public string Slug { get; set; } = null!;

        [JsonPropertyName("position")] public int Position { get; set; }
    }

    public class LiveList
    {
        [JsonPropertyName("data")] public LiveListDatum[] Data { get; set; } = null!;

        [JsonPropertyName("meta")] public LiveListMeta Meta { get; set; } = null!;
    }

    public class LiveListDatum
    {
        [JsonPropertyName("id")] public Guid Id { get; set; }

        [JsonPropertyName("status")] public string Status { get; set; } = null!;

        [JsonPropertyName("event_date")] public string EventDate { get; set; } = null!;

        [JsonPropertyName("event_date_status")]
        public string EventDateStatus { get; set; } = null!;

        [JsonPropertyName("category")] public CategoryElement Category { get; set; } = null!;

        [JsonPropertyName("venue")] public string Venue { get; set; } = null!;

        [JsonPropertyName("title")] public string Title { get; set; } = null!;

        [JsonPropertyName("artist")] public string Artist { get; set; } = null!;

        [JsonPropertyName("doors_starts_time")]
        public string DoorsStartsTime { get; set; } = null!;

        [JsonPropertyName("price")] public string Price { get; set; } = null!;

        [JsonPropertyName("reservation_setting")]
        public ReservationSetting ReservationSetting { get; set; } = null!;

        [JsonPropertyName("body")] public object Body { get; set; } = null!;

        [JsonPropertyName("publishes_at")] public DateTimeOffset PublishesAt { get; set; }

        [JsonPropertyName("archived")] public int Archived { get; set; }

        [JsonPropertyName("created_at")] public DateTimeOffset CreatedAt { get; set; }

        [JsonPropertyName("updated_at")] public DateTimeOffset UpdatedAt { get; set; }
    }

    public class ReservationSetting
    {
        [JsonPropertyName("ticket_reservation_type")]
        public string TicketReservationType { get; set; } = null!;

        [JsonPropertyName("web_reservation_max_quantity")]
        public object WebReservationMaxQuantity { get; set; } = null!;

        [JsonPropertyName("web_reservation_max_quantity_per_person")]
        public object WebReservationMaxQuantityPerPerson { get; set; } = null!;

        [JsonPropertyName("platforms")] public Platform[] Platforms { get; set; } = null!;
    }

    public class Platform
    {
        [JsonPropertyName("id")] public string Id { get; set; } = null!;

        [JsonPropertyName("url")] public Uri Url { get; set; } = null!;
    }

    public class LiveListMeta
    {
        [JsonPropertyName("current_page")] public int CurrentPage { get; set; }

        [JsonPropertyName("from")] public int From { get; set; }

        [JsonPropertyName("last_page")] public int LastPage { get; set; }

        [JsonPropertyName("links")] public Link[] Links { get; set; } = null!;

        [JsonPropertyName("path")] public Uri Path { get; set; } = null!;

        [JsonPropertyName("per_page")] public int PerPage { get; set; }

        [JsonPropertyName("to")] public int To { get; set; }

        [JsonPropertyName("total")] public int Total { get; set; }
    }

    public class Link
    {
        [JsonPropertyName("url")] public Uri Url { get; set; } = null!;

        [JsonPropertyName("label")] public string Label { get; set; } = null!;

        [JsonPropertyName("active")] public bool Active { get; set; }
    }
}
