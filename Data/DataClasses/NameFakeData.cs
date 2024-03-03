using Bogus.DataSets;
using Flow.Launcher.Plugin.FakeData.Data.Attributes;
using JetBrains.Annotations;

#pragma warning disable CS1591

namespace Flow.Launcher.Plugin.FakeData.Data.DataClasses;

public class NameFakeData {
    [FullSearch] public Name.Gender? Gender { get; [UsedImplicitly] set; }

    [UsedImplicitly]
    public void SetGender(string input) {
        Gender = input.ToLower() switch {
            "m" or "male" => Name.Gender.Male,
            "f" or "female" => Name.Gender.Female,
            _ => null,
        };
    }
}