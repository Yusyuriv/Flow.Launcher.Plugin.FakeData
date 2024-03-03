using Flow.Launcher.Plugin.FakeData.Data.Attributes;
using JetBrains.Annotations;

#pragma warning disable CS1591

namespace Flow.Launcher.Plugin.FakeData.Data.DataClasses;

public class CommerceFakeData {
    [Rename("min")] public decimal MinPrice { get; [UsedImplicitly] set; } = 1;
    [Rename("max")] public decimal MaxPrice { get; [UsedImplicitly] set; } = 1000;
    public int Decimals { get; [UsedImplicitly] set; } = 2;
    [FullSearch] public string Symbol { get; [UsedImplicitly] set; } = "";
}