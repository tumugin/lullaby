namespace Lullaby.Crawler.Events;

public class EventTypeDetector
{
    private readonly IEnumerable<string> battleEventNames =
        new[] { "HYPE", "Funpal", "MAWA LOOP", "アイドルアラモード", "MARQUEE祭", "dot yell", "TOKYO GIRLS GIRLS" };

    private readonly IEnumerable<string> fesEventNames =
        new[]
        {
            "LEADING PREMIUM", "TIF", "TOKYO IDOL FESTIVAL", "NATSUZOME", "エンドレスサマー", "@JAM",
            "SEKIGAHARA IDOL WARS", "関ケ原唄姫合戦", "SPARK", "IDORISE"
        };

    public EventType DetectEventTypeByTitle(string eventTitle) =>
        eventTitle switch
        {
            not null when this.battleEventNames.Any(eventTitle.Contains) => EventType.Battle,
            not null when this.fesEventNames.Any(eventTitle.Contains) => EventType.Fes,
            not null when eventTitle.Contains("対バン") => EventType.Battle,
            _ => EventType.Unknown
        };
}
