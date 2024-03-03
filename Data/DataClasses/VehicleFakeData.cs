using Flow.Launcher.Plugin.FakeData.Data.Attributes;
using JetBrains.Annotations;

#pragma warning disable CS1591
namespace Flow.Launcher.Plugin.FakeData.Data.DataClasses;

public class VehicleFakeData {
    [FullSearch] public bool Strict { [UsedImplicitly] get; private set; }

    [UsedImplicitly]
    public void SetStrict(string input) {
        Strict = input.ToLower() switch {
            "s" or "strict" => true,
            _ => Parser.Parser.ParseBool(input),
        };
    }
}
