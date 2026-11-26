// Actividad que recibe la redirección de Google al terminar el login (Android)
using Android.App;
using Android.Content;
using Android.Content.PM;
using AppFinalAv4A.Features.auth.Service;
using Microsoft.Maui.Authentication;

namespace AppFinalAv4A;

[Activity(
    NoHistory = true,
    LaunchMode = LaunchMode.SingleTop,
    Exported = true)]
[IntentFilter(
    new[] { Intent.ActionView },
    Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
    DataScheme = GoogleAuthConfig.RedirectScheme,
    DataPath = "/oauth2redirect")]
public class WebAuthenticationCallbackActivity : WebAuthenticatorCallbackActivity
{
}