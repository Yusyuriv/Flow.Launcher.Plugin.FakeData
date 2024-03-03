using Bogus.DataSets;
using Flow.Launcher.Plugin.FakeData.Data.Attributes;
using JetBrains.Annotations;

#pragma warning disable CS1591

namespace Flow.Launcher.Plugin.FakeData.Data.DataClasses;

public class AddressFakeData {
    [Rename("zip")] [FullSearch] public string ZipCodeFormat { get; [UsedImplicitly] set; }

    [Rename("format")]
    [FullSearch]
    public Iso3166Format CountryCodeFormat { get; [UsedImplicitly] set; } = Iso3166Format.Alpha2;

    [Rename("full")] [FullSearch] public bool UseFullAddress { get; [UsedImplicitly] set; }

    [Rename("min")] public double LatitudeMin { get; [UsedImplicitly] set; } = -90;
    [Rename("max")] public double LatitudeMax { get; [UsedImplicitly] set; } = 90;
    [Rename("min")] public double LongitudeMin { get; [UsedImplicitly] set; } = -180;
    [Rename("max")] public double LongitudeMax { get; [UsedImplicitly] set; } = 180;
    [Rename("abbr")] [FullSearch] public bool DirectionAbbreviation { get; [UsedImplicitly] set; }

    [UsedImplicitly]
    public void SetCountryCodeFormat(string input) {
        CountryCodeFormat = input.ToLower() switch {
            "alpha3" or "3" => Iso3166Format.Alpha3,
            "alpha2" or "2" => Iso3166Format.Alpha2,
            _ => Iso3166Format.Alpha2,
        };
    }

    [UsedImplicitly]
    public void SetUseFullAddress(string input) {
        UseFullAddress = input.ToLower() switch {
            "full" => true,
            _ => Parser.Parser.ParseBool(input),
        };
    }

    [UsedImplicitly]
    public void SetAbbreviation(string input) {
        DirectionAbbreviation = input.ToLower() switch {
            "abbr" => true,
            _ => Parser.Parser.ParseBool(input),
        };
    }
}