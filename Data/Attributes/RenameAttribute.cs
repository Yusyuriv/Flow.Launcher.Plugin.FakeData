using System;

#pragma warning disable CS1591

namespace Flow.Launcher.Plugin.FakeData.Data.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class RenameAttribute : Attribute {
    public string[] NewNames { get; }

    public RenameAttribute(params string[] newNames) {
        NewNames = newNames;
    }
}
