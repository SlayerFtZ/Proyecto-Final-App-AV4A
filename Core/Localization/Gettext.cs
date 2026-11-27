namespace AppFinalAv4A.Core.Localization;

public static class Gettext
{
  
    public static string _(string text)
        => LocalizationService.Instance.GetString(text);

  
    public static string _(string text, params object[] args)
        => LocalizationService.Instance.GetString(text, args);

 
    public static string _n(string text, string plural, long n)
        => LocalizationService.Instance.GetPluralString(text, plural, n);


    public static string _p(string context, string text)
        => LocalizationService.Instance.GetParticularString(context, text);
}