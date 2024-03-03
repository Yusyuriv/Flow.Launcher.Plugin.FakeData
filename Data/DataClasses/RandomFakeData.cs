using Flow.Launcher.Plugin.FakeData.Data.Attributes;
using JetBrains.Annotations;

#pragma warning disable CS1591
namespace Flow.Launcher.Plugin.FakeData.Data.DataClasses;

public class RandomFakeData {
    [Rename("min")] public int MinInt { get; [UsedImplicitly] set; } = 0;
    [Rename("max")] public int MaxInt { get; [UsedImplicitly] set; } = 100;
    [Rename("min")] public double MinDouble { get; [UsedImplicitly] set; } = 0;
    [Rename("max")] public double MaxDouble { get; [UsedImplicitly] set; } = 100;
    [Rename("min")] public decimal MinDecimal { get; [UsedImplicitly] set; } = 0;
    [Rename("max")] public decimal MaxDecimal { get; [UsedImplicitly] set; } = 100;
    [Rename("min")] public float MinFloat { get; [UsedImplicitly] set; } = 0;
    [Rename("max")] public float MaxFloat { get; [UsedImplicitly] set; } = 100;
    [Rename("min")] public byte MinByte { get; [UsedImplicitly] set; } = 0;
    [Rename("max")] public byte MaxByte { get; [UsedImplicitly] set; } = 255;
    [Rename("min")] public sbyte MinSByte { get; [UsedImplicitly] set; } = -128;
    [Rename("max")] public sbyte MaxSByte { get; [UsedImplicitly] set; } = 127;
    [Rename("min")] public uint MinUInt { get; [UsedImplicitly] set; } = 0;
    [Rename("max")] public uint MaxUInt { get; [UsedImplicitly] set; } = 100;
    [Rename("min")] public ulong MinULong { get; [UsedImplicitly] set; } = 0;
    [Rename("max")] public ulong MaxULong { get; [UsedImplicitly] set; } = 100;
    [Rename("min")] public long MinLong { get; [UsedImplicitly] set; } = 0;
    [Rename("max")] public long MaxLong { get; [UsedImplicitly] set; } = 100;
    [Rename("min")] public short MinShort { get; [UsedImplicitly] set; } = 0;
    [Rename("max")] public short MaxShort { get; [UsedImplicitly] set; } = 100;
    [Rename("min")] public ushort MinUShort { get; [UsedImplicitly] set; } = 0;
    [Rename("max")] public ushort MaxUShort { get; [UsedImplicitly] set; } = 100;
    public int Length { get; [UsedImplicitly] set; } = 100;
    [Rename("upper")] [FullSearch] public bool Uppercase { get; [UsedImplicitly] set; }
    public char Symbol { get; [UsedImplicitly] set; } = '#';
    [FullSearch] public string Prefix { get; [UsedImplicitly] set; } = "0x";

    [UsedImplicitly]
    public void SetUppercase(string input) {
        Uppercase = input.ToLower() switch {
            "u" or "upper" => true,
            _ => Parser.Parser.ParseBool(input),
        };
    }
}
