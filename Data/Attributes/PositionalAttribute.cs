using System;

#pragma warning disable CS1591

namespace Flow.Launcher.Plugin.FakeData.Data.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class PositionalAttribute : Attribute {
    public int Position { get; }

    public PositionalAttribute(int position) {
        Position = position;
    }
}