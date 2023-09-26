namespace Lullaby.Crawler.Utility;

using System.Globalization;
using System.Text.RegularExpressions;

public partial class FullDateGuesser : IFullDateGuesser
{
    /// <inheritdoc />
    public IReadOnlyList<DateTimeOffset> GuessFullDateByUncompletedDate(
        IReadOnlyList<string> uncompletedDate,
        DateTimeOffset currentTime
    )
    {
        var parsedDates = uncompletedDate.Select(v =>
            {
                // get month and date in regex
                var match = MonthDateRegex().Match(v);
                var month = int.Parse(match.Groups["month"].Value, CultureInfo.InvariantCulture);
                var date = int.Parse(match.Groups["date"].Value, CultureInfo.InvariantCulture);
                return new { Month = month, Date = date };
            })
            .ToArray();

        if (parsedDates.Length == 0)
        {
            return Array.Empty<DateTimeOffset>();
        }

        var isFirstDateInCurrentYear =
            // in the same month
            parsedDates.First().Month == currentTime.Month
            // or, is in the range of 6 months
            || Math.Abs(currentTime.Month - parsedDates.First().Month) <= 6;
        var currentYearOfUncompletedCalendar =
            isFirstDateInCurrentYear ? currentTime.Year : currentTime.Year - 1;
        var nextYearOfUncompletedCalendar = currentYearOfUncompletedCalendar + 1;

        return parsedDates.Select(v => new DateTimeOffset(
                v.Month >= parsedDates.First().Month ? currentYearOfUncompletedCalendar : nextYearOfUncompletedCalendar,
                v.Month,
                v.Date,
                0,
                0,
                0,
                currentTime.Offset
            ))
            .ToArray();
    }

    [GeneratedRegex("(?<month>\\d{1,2})/(?<date>\\d{1,2})")]
    private static partial Regex MonthDateRegex();
}
