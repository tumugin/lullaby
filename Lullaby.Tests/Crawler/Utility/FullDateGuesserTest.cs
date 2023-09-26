namespace Lullaby.Tests.Crawler.Utility;

using System.Globalization;
using Lullaby.Crawler.Utility;

public class FullDateGuesserTest
{
    [TestCase(
        "2023-09-10T13:50:40+09:00",
        new[] { "9/1", "9/02", "9/10", "10/1", "10/02", "10/10", "1/1", "1/10" },
        new[]
        {
            "2023-09-01T00:00:00+09:00", "2023-09-02T00:00:00+09:00", "2023-09-10T00:00:00+09:00",
            "2023-10-01T00:00:00+09:00", "2023-10-02T00:00:00+09:00", "2023-10-10T00:00:00+09:00",
            "2024-01-01T00:00:00+09:00", "2024-01-10T00:00:00+09:00"
        }
    )]
    [TestCase(
        "2023-10-01T13:50:40+09:00",
        new[] { "9/1", "9/02", "9/10", "10/1", "10/02", "10/10", "1/1", "1/10" },
        new[]
        {
            "2023-09-01T00:00:00+09:00", "2023-09-02T00:00:00+09:00", "2023-09-10T00:00:00+09:00",
            "2023-10-01T00:00:00+09:00", "2023-10-02T00:00:00+09:00", "2023-10-10T00:00:00+09:00",
            "2024-01-01T00:00:00+09:00", "2024-01-10T00:00:00+09:00"
        }
    )]
    [TestCase(
        "2024-01-01T13:50:40+09:00",
        new[] { "9/1", "9/02", "9/10", "10/1", "10/02", "10/10", "1/1", "1/10" },
        new[]
        {
            "2023-09-01T00:00:00+09:00", "2023-09-02T00:00:00+09:00", "2023-09-10T00:00:00+09:00",
            "2023-10-01T00:00:00+09:00", "2023-10-02T00:00:00+09:00", "2023-10-10T00:00:00+09:00",
            "2024-01-01T00:00:00+09:00", "2024-01-10T00:00:00+09:00"
        }
    )]
    public void GuessFullDateByUncompletedDateTest(
        string currentDateTime,
        IReadOnlyList<string> uncompletedDate,
        IReadOnlyList<string> expected
    )
    {
        var currentTime = DateTimeOffset.Parse(currentDateTime, CultureInfo.InvariantCulture);
        var guesser = new FullDateGuesser();
        var actual = guesser.GuessFullDateByUncompletedDate(uncompletedDate, currentTime);
        var convertedExpected = expected
            .Select(v => DateTimeOffset.Parse(v, CultureInfo.InvariantCulture))
            .ToArray();
        Assert.That(actual, Is.EqualTo(convertedExpected));
    }
}
