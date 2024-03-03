using Bogus.DataSets;
using Flow.Launcher.Plugin.FakeData.Data.Attributes;
using JetBrains.Annotations;

#pragma warning disable CS1591

namespace Flow.Launcher.Plugin.FakeData.Data.DataClasses;

public class InternetFakeData {
    [Rename("first")] public string FirstName { get; [UsedImplicitly] set; } = null;
    [Rename("last")] public string LastName { get; [UsedImplicitly] set; } = null;
    public string Provider { get; [UsedImplicitly] set; } = null;
    public string Suffix { get; [UsedImplicitly] set; } = null;
    [Rename("sep")] public string Separator { get; [UsedImplicitly] set; } = ":";
    public int Length { get; [UsedImplicitly] set; } = 10;
    [Rename("mem")] [FullSearch] public bool Memorable { get; [UsedImplicitly] set; }
    public byte Red { get; [UsedImplicitly] set; } = 0;
    public byte Green { get; [UsedImplicitly] set; } = 0;
    public byte Blue { get; [UsedImplicitly] set; } = 0;
    [Rename("gray", "grey")] public bool Grayscale { get; [UsedImplicitly] set; } = false;
    public ColorFormat Format { get; [UsedImplicitly] set; } = ColorFormat.Hex;
    public string Protocol { get; [UsedImplicitly] set; } = null;
    public string Domain { get; [UsedImplicitly] set; } = null;
    [Rename("ext", "fileext")] public string FileExtension { get; [UsedImplicitly] set; } = null;

    [UsedImplicitly]
    public void SetFormat(string format) {
        Format = format.ToLower() switch {
            "hex" => ColorFormat.Hex,
            "rgb" => ColorFormat.Rgb,
            "delim" or "delimited" => ColorFormat.Delimited,
            _ => ColorFormat.Hex,
        };
    }

    [UsedImplicitly]
    public void SetMemorable(string input) {
        Memorable = input.ToLower() switch {
            "mem" or "memorable" => true,
            _ => Parser.Parser.ParseBool(input),
        };
    }
}
