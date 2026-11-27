using System.ComponentModel;
using System.Globalization;
using NGettext;

namespace AppFinalAv4A.Core.Localization;

public class LocalizationService : ILocalizationService
{
    private const string PrefKey = "app_culture";

    public static LocalizationService Instance { get; } = new();

    private ICatalog _catalog = new Catalog(CultureInfo.InvariantCulture);

    public event PropertyChangedEventHandler? PropertyChanged;

    public CultureInfo CurrentCulture { get; private set; } = CultureInfo.CurrentUICulture;

    public async Task InitializeAsync()
    {
        var saved = Preferences.Get(PrefKey, string.Empty);
        var culture = string.IsNullOrEmpty(saved)
            ? CultureInfo.CurrentUICulture
            : new CultureInfo(saved);

        await ApplyAsync(culture);
        
    }

    public async Task SetCultureAsync(string cultureName)
    {
        Preferences.Set(PrefKey, cultureName);
        await ApplyAsync(new CultureInfo(cultureName));
    }

    public string GetString(string text) => _catalog.GetString(text);

    public string GetString(string text, params object[] args) => _catalog.GetString(text, args);

    public string GetPluralString(string text, string pluralText, long n)
        => _catalog.GetPluralString(text, pluralText, n);

    public string GetParticularString(string context, string text)
        => _catalog.GetParticularString(context, text);

    private async Task ApplyAsync(CultureInfo culture)
    {
        _catalog = await LoadCatalogAsync(culture);

        CurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentCulture = culture;
        CultureInfo.DefaultThreadCurrentUICulture = culture;
        CultureInfo.CurrentUICulture = culture;

       System.Diagnostics.Debug.WriteLine($"[i18n] Cultura aplicada: {culture.Name}");

        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentCulture)));
    }

    private static async Task<ICatalog> LoadCatalogAsync(CultureInfo culture)
    {
       
        var candidates = new[] { culture.Name, culture.TwoLetterISOLanguageName }
            .Where(n => !string.IsNullOrEmpty(n))
            .Distinct();

        foreach (var name in candidates)
        {
            try
            {
                using var asset = await FileSystem.OpenAppPackageFileAsync($"locale/{name}/messages.mo");
                var ms = new MemoryStream();
                await asset.CopyToAsync(ms);
                ms.Position = 0;
                return new Catalog(ms, culture);
            }
            catch (Exception ex)
{
    System.Diagnostics.Debug.WriteLine($"[i18n] FALLÓ locale/{name}/messages.mo -> {ex.GetType().Name}: {ex.Message}");
}
        }

      
        return new Catalog(culture);
    }
}