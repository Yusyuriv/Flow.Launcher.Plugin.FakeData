using Flow.Launcher.Plugin.FakeData.Data.Attributes;
using JetBrains.Annotations;

#pragma warning disable CS1591

namespace Flow.Launcher.Plugin.FakeData.Data.DataClasses;

public class PhoneFakeData {
    [FullSearch] public string Format { get; [UsedImplicitly] set; }
    [FullSearch] public int Index { get; [UsedImplicitly] set; }
}