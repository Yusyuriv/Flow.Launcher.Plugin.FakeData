using Flow.Launcher.Plugin.FakeData.Data.Attributes;
using JetBrains.Annotations;

#pragma warning disable CS1591

namespace Flow.Launcher.Plugin.FakeData.Data.DataClasses;

public class ImagesFakeData {
    [Rename("w")] public int Width { get; [UsedImplicitly] set; } = 640;
    [Rename("h")] public int Height { get; [UsedImplicitly] set; } = 480;
    public string Color { get; [UsedImplicitly] set; } = "grey";
    [Rename("gray", "grey")] public bool Greyscale { get; [UsedImplicitly] set; } = false;
    public bool Blur { get; [UsedImplicitly] set; } = false;
    public string Keywords { get; [UsedImplicitly] set; } = null;

    [Rename("match_all", "all")]
    [UsedImplicitly]
    public bool MatchAllKeywords { get; set; }
}
