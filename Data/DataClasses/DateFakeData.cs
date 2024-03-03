using System;
using Flow.Launcher.Plugin.FakeData.Data.Attributes;
using JetBrains.Annotations;

#pragma warning disable CS1591

namespace Flow.Launcher.Plugin.FakeData.Data.DataClasses;

public class DateFakeData {
    public int Years { get; [UsedImplicitly] set; } = 1;
    public int Days { get; [UsedImplicitly] set; } = 1;
    public DateTime From { get; [UsedImplicitly] set; } = DateTime.Now.AddYears(-1);
    public DateTime To { get; [UsedImplicitly] set; } = DateTime.Now;
    [Rename("abbr")] [FullSearch] public bool Abbreviate { get; [UsedImplicitly] set; } = false;
}