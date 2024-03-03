using Flow.Launcher.Plugin.FakeData.Data.Attributes;
using JetBrains.Annotations;

#pragma warning disable CS1591
namespace Flow.Launcher.Plugin.FakeData.Data.DataClasses;

public class CommonFakeData {
    private string _locale = "en";
    private int _repeat = 1;
    [Rename("__search__")] public string Search { get; [UsedImplicitly] set; } = "";

    [Rename("lang")]
    public string Locale {
        get => _locale;
        [UsedImplicitly]
        set {
            if (value.Length != 2) return;
            _locale = value;
        }
    }

    public int Repeat {
        get => _repeat;
        [UsedImplicitly]
        set {
            if (value is < 1 or > 100) return;
            _repeat = value;
        }
    }
}