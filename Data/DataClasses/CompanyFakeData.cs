using Flow.Launcher.Plugin.FakeData.Data.Attributes;
using JetBrains.Annotations;

#pragma warning disable CS1591

namespace Flow.Launcher.Plugin.FakeData.Data.DataClasses;

public class CompanyFakeData {
    [FullSearch] public int? Format { get; [UsedImplicitly] set; } = null;
}