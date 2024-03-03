using Flow.Launcher.Plugin.FakeData.Data.Attributes;
using JetBrains.Annotations;

#pragma warning disable CS1591

namespace Flow.Launcher.Plugin.FakeData.Data.DataClasses;

public class LoremFakeData {
    [Positional(0)] public int Words { get; [UsedImplicitly] set; } = 10;
    [Positional(1)] public int Range { get; [UsedImplicitly] set; } = 0;
    [FullSearch] public int Sentences { get; [UsedImplicitly] set; } = 3;
    [FullSearch] public int Lines { get; [UsedImplicitly] set; } = 3;
}
