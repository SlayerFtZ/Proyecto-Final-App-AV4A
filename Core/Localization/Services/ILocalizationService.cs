using System.ComponentModel;
using System.Globalization;

namespace AppFinalAv4A.Core.Localization;

public interface ILocalizationService : INotifyPropertyChanged
{
    CultureInfo CurrentCulture { get; }

    Task InitializeAsync();
    Task SetCultureAsync(string cultureName);

    string GetString(string text);
    string GetString(string text, params object[] args);
    string GetPluralString(string text, string pluralText, long n);
    string GetParticularString(string context, string text);
}