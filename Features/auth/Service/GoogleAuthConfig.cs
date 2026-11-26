// Configuración de Google Auth: cliente y endpoints
namespace AppFinalAv4A.Features.auth.Service
{
    public static class GoogleAuthConfig
    {
        public const string ClientId = "513171707960-coe1kpl8j89uvj1p9rl6veg20nlc4ntl.apps.googleusercontent.com";

        public const string RedirectScheme = "com.companyname.appfinalav4a";
        public const string RedirectUri = RedirectScheme + ":/oauth2redirect";

        public const string AuthEndpoint = "https://accounts.google.com/o/oauth2/v2/auth";
        public const string TokenEndpoint = "https://oauth2.googleapis.com/token";
        public const string UserInfoEndpoint = "https://openidconnect.googleapis.com/v1/userinfo";
        public const string Scopes = "openid email profile";
    }
}