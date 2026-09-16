using Server_Strategico.ServerData.Localization;
using static Server_Strategico.Gioco.Giocatori;

internal static class LocalizationManager
{
    private static readonly Dictionary<string, ILocalization> _lingue = new()
    {
        { "it", new ITA() },
        { "en", new ENG() },
    };

    public static ILocalization Get(Player player) =>
        _lingue.TryGetValue(player.Lingua, out var loc) ? loc : _lingue["it"];
}