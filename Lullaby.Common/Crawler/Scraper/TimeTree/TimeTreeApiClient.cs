namespace Lullaby.Common.Crawler.Scraper.TimeTree;

using System.Text.Json;
using System.Text.Json.Serialization;
using Flurl;

public class TimeTreeApiClient : ITimeTreeApiClient
{
    private readonly HttpClient httpClient;
    private readonly string timeTreeApiBaseUrl = "https://timetreeapp.com/";

    public TimeTreeApiClient(HttpClient httpClient) => this.httpClient = httpClient;

    public async Task<TimeTreeApiResult> GetEventsAsync(
        string calendarId,
        DateTimeOffset startDate,
        DateTimeOffset endDate,
        string? pageCursor,
        CancellationToken cancellationToken
    )
    {
        // validate the offsets
        if (startDate.Offset != endDate.Offset)
        {
            throw new ArgumentException(
                "The offsets of the start date and the end date must be the same.",
                nameof(startDate)
            );
        }

        var requestUri = this.timeTreeApiBaseUrl
            .AppendPathSegments("api", "v2", "public_calendars", calendarId, "public_events")
            .SetQueryParams(new
            {
                cursor = pageCursor,
                // Unix time in milliseconds
                from = startDate.ToUnixTimeMilliseconds(),
                // Unix time in milliseconds
                to = endDate.ToUnixTimeMilliseconds(),
                // Offset in seconds from UTC
                utc_offset = startDate.Offset.Seconds
            })
            .ToUri();

        using var httpRequestMessage = new HttpRequestMessage();
        httpRequestMessage.RequestUri = requestUri;
        httpRequestMessage.Method = HttpMethod.Get;
        // Set same as the web client
        httpRequestMessage.Headers.Add("x-timetreea", "web/2.1.0/ja");
        httpRequestMessage.Headers.Add("user-agent", "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/133.0.0.0 Safari/537.36 Edg/133.0.0.0");

        using var rawResponse = await this.httpClient.SendAsync(httpRequestMessage, cancellationToken);
        var rawJson = await rawResponse.Content.ReadAsStringAsync(cancellationToken);
        var rawResult = JsonSerializer.Deserialize<TimeTreeApiRawResult.Root>(rawJson);

        if (rawResult == null)
        {
            throw new JsonException("The result must not be null.");
        }

        return ConvertRawResultToTimeTreeApiResult(rawResult);
    }

    private static TimeTreeApiResult ConvertRawResultToTimeTreeApiResult(TimeTreeApiRawResult.Root root) =>
        new()
        {
            // Somehow, the cursor is set even if there is no next page.
            NextPageCursor = root.Paging.Next ? root.Paging.NextCursor : null,
            HasNextPage = root.Paging.Next,
            Schedules = root.PublicEvents.Select(v => new TimeTreeApiResult.TimeTreeSchedule
            {
                Id = v.Id,
                Title = v.Title,
                Description = v.Note,
                ImageUrls = v.Images.Overview?.Select(x => x.Url).ToArray() ?? [],
                LocationName = v.LocationName != "" ? v.LocationName : null,
                // UnixTime in milliseconds should be in UTC but it seems to be in local timezone. We have to forcibly set the timezone.
                StartAt = ForciblySetTimeZoneToDateTimeOffset(
                    DateTimeOffset.FromUnixTimeMilliseconds(v.StartAt),
                    v.StartTimezone
                ),
                EndAt = ForciblySetTimeZoneToDateTimeOffset(
                    DateTimeOffset.FromUnixTimeMilliseconds(v.EndAt),
                    v.EndTimezone
                ),
                UntilAt = ForciblySetTimeZoneToDateTimeOffset(
                    DateTimeOffset.FromUnixTimeMilliseconds(v.UntilAt),
                    v.EndTimezone
                )
            }).ToArray()
        };

    private static DateTimeOffset ForciblySetTimeZoneToDateTimeOffset(DateTimeOffset dateTimeOffset, string timezone)
    {
        var timezoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timezone);
        return new DateTimeOffset(dateTimeOffset.DateTime, timezoneInfo.BaseUtcOffset);
    }

    private sealed class TimeTreeApiRawResult
    {
        public sealed record Attachment;

        public sealed record Cover(
            [property: JsonPropertyName("url")] string Url,
            [property: JsonPropertyName("thumbnail_url")]
            string ThumbnailUrl
        );

        public sealed record Images(
            [property: JsonPropertyName("cover")] IReadOnlyList<Cover> Cover,
            [property: JsonPropertyName("overview")]
            IReadOnlyList<Overview>? Overview
        );

        public sealed record Location(
            [property: JsonPropertyName("title")] string Title,
            [property: JsonPropertyName("address")]
            string Address,
            [property: JsonPropertyName("access")] string Access,
            [property: JsonPropertyName("url")] string Url,
            [property: JsonPropertyName("note")] string Note
        );

        public sealed record Overview(
            [property: JsonPropertyName("url")] string Url,
            [property: JsonPropertyName("thumbnail_url")]
            string ThumbnailUrl
        );

        public sealed record Paging(
            [property: JsonPropertyName("next_cursor")]
            string NextCursor,
            [property: JsonPropertyName("next")] bool Next
        );

        public sealed record PublicEvent(
            [property: JsonPropertyName("id")] string Id,
            [property: JsonPropertyName("public_calendar_id")]
            int PublicCalendarId,
            [property: JsonPropertyName("title")] string Title,
            [property: JsonPropertyName("overview")]
            string Overview,
            [property: JsonPropertyName("note")]
            string Note,
            [property: JsonPropertyName("images")] Images Images,
            [property: JsonPropertyName("videos")] IReadOnlyList<object> Videos,
            [property: JsonPropertyName("status")] int Status,
            [property: JsonPropertyName("target")] string Target,
            [property: JsonPropertyName("color")] int Color,
            [property: JsonPropertyName("location")]
            Location Location,
            [property: JsonPropertyName("location_name")]
            string LocationName,
            [property: JsonPropertyName("location_address")]
            string LocationAddress,
            [property: JsonPropertyName("location_access")]
            string LocationAccess,
            [property: JsonPropertyName("location_url")]
            string LocationUrl,
            [property: JsonPropertyName("location_note")]
            string LocationNote,
            [property: JsonPropertyName("location_lat")]
            object LocationLat,
            [property: JsonPropertyName("location_lon")]
            object LocationLon,
            [property: JsonPropertyName("all_day")]
            bool AllDay,
            [property: JsonPropertyName("start_at")]
            long StartAt,
            [property: JsonPropertyName("end_at")] long EndAt,
            [property: JsonPropertyName("start_timezone")]
            string StartTimezone,
            [property: JsonPropertyName("end_timezone")]
            string EndTimezone,
            [property: JsonPropertyName("recurrences")]
            object Recurrences,
            [property: JsonPropertyName("until_at")]
            long UntilAt,
            [property: JsonPropertyName("region_timezone")]
            string RegionTimezone,
            [property: JsonPropertyName("period_closed")]
            string PeriodClosed,
            [property: JsonPropertyName("period_note")]
            string PeriodNote,
            [property: JsonPropertyName("url")] string Url,
            [property: JsonPropertyName("attachment")]
            Attachment Attachment,
            [property: JsonPropertyName("updated_at")]
            long UpdatedAt,
            [property: JsonPropertyName("created_at")]
            long CreatedAt,
            [property: JsonPropertyName("summary")]
            Summary Summary
        );

        public sealed record Root(
            [property: JsonPropertyName("paging")] Paging Paging,
            [property: JsonPropertyName("public_events")]
            IReadOnlyList<PublicEvent> PublicEvents
        );

        public sealed record Summary(
            [property: JsonPropertyName("total_event_count")]
            int TotalEventCount,
            [property: JsonPropertyName("like_count")]
            int LikeCount,
            [property: JsonPropertyName("liked")] bool Liked
        );
    }
}
