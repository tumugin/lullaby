namespace Lullaby.Tests.Crawler.Events;

using Common.Crawler.Events;
using Common.Enums;

public class EventTypeDetectorTest
{
    [Test, Combinatorial] public void TestBattleEventType(
        [Values("MAWA LOOP TOKYO 2022",
            "MARQUEE祭 Vol.122",
            "dot yell fes Summer SP")]
        string eventName
    )
    {
        var detector = new EventTypeDetector();
        Assert.That(detector.DetectEventTypeByTitle(eventName), Is.EqualTo(EventType.Battle));
    }

    [Test, Combinatorial] public void TestFesEventType(
        [Values("SPARK 2022 in YAMANAKAKO",
            "超NATSUZOME2022",
            "TOKYO IDOL FESTIVAL 2022 supported by にしたんクリニック",
            "IDORISE!! FESTIVAL 2023")]
        string eventName
    )
    {
        var detector = new EventTypeDetector();
        Assert.That(detector.DetectEventTypeByTitle(eventName), Is.EqualTo(EventType.Fes));
    }

    [Test, Combinatorial] public void TestNotDefinedType(
        [Values(
            "小野寺梓生誕祭2022　〜おのでらんどの夏休み〜",
            "真っ白なキャンバス定期公演vol.5 〜5年後も10年後もこんな毎日が続けばいいのにね〜")]
        string eventName
    )
    {
        var detector = new EventTypeDetector();
        Assert.That(detector.DetectEventTypeByTitle(eventName), Is.EqualTo(EventType.Unknown));
    }
}
