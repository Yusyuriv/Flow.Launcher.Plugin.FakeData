using Bogus.DataSets;
using Flow.Launcher.Plugin.FakeData.Data.Attributes;
using JetBrains.Annotations;

#pragma warning disable CS1591

namespace Flow.Launcher.Plugin.FakeData.Data.DataClasses;

public class FinanceFakeData {
    [FullSearch] public int Length { get; [UsedImplicitly] set; } = 8;
    public decimal Min { get; [UsedImplicitly] set; } = 0;
    public decimal Max { get; [UsedImplicitly] set; } = 1000;
    public int Decimals { get; [UsedImplicitly] set; } = 2;
    [Rename("card")] [FullSearch] public CardType CardType { get; [UsedImplicitly] set; } = CardType.Visa;
    [Rename("fund")] [FullSearch] public bool IncludeFundCodes { get; [UsedImplicitly] set; } = false;

    [UsedImplicitly]
    public void SetCardType(string input) {
        CardType = input.ToLower() switch {
            "visa" => CardType.Visa,
            "mastercard" => CardType.Mastercard,
            "discover" => CardType.Discover,
            "amex" or "americanexpress" => CardType.AmericanExpress,
            "diners" or "dinersclub" => CardType.DinersClub,
            "jcb" => CardType.Jcb,
            "instapayment" => CardType.Instapayment,
            "laser" => CardType.Laser,
            "solo" => CardType.Solo,
            "maestro" => CardType.Maestro,
            "switch" => CardType.Switch,
            _ => null,
        };
    }
}