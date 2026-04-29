using Server_Strategico.ServerData.Localization;
using static Server_Strategico.Gioco.Giocatori;

internal static class LocalizationManager
{
    private static readonly Dictionary<string, ILocalization> _lingue = new()
    {
        { "ITA", new ITA() },
        { "ENG", new ENG() },
    };

    public static ILocalization Get(Player player) =>
        _lingue.TryGetValue(player.Lingua, out var loc) ? loc : _lingue["ITA"];
}