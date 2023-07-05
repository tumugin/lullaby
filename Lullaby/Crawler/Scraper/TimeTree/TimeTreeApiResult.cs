namespace Lullaby.Crawler.Scraper.TimeTree;

using System.Diagnostics.CodeAnalysis;

public class TimeTreeApiResult
{
    public required string? NextPageCursor { get; init; }

    [MemberNotNullWhen(true, nameof(NextPageCursor))]
    public required bool HasNextPage { get; init; }

    public required IReadOnlyList<TimeTreeSchedule> Schedules { get; init; }

    public class TimeTreeSchedule
    {
        public required string Id { get; init; }
        public required string Title { get; init; }
        public required string Overview { get; init; }
        public required IReadOnlyList<string> ImageUrls { get; init; }
        public required string? LocationName { get; init; }
        public required DateTimeOffset StartAt { get; init; }
        public required DateTimeOffset EndAt { get; init; }
        public required DateTimeOffset UntilAt { get; init; }
    }
}
