using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using AppFinalAv4A.Features.auth.Domain.Datasource;
using AppFinalAv4A.Features.auth.Domain.Repositories;
using AppFinalAv4A.Features.auth.Infrastructure.Datasource;
using AppFinalAv4A.Features.auth.Infrastructure.Repositories;
using AppFinalAv4A.Features.auth.Service;
using AppFinalAv4A.Features.auth.Presentation.Screens;
using AppFinalAv4A.Core.Localization;
using AppFinalAv4A.Features.Language.Screens;
using FastPopups;
using FastPopups;
using AppFinalAv4A.Features.Shared.Presentation.Dialogs.Interfaces;
using AppFinalAv4A.Features.Shared.Presentation.Dialogs.Services;
using SkiaSharp.Views.Maui.Controls.Hosting;
using AppFinalAv4A.Core.Network;
using AppFinalAv4A.Features.home.Domain.Datasource;
using AppFinalAv4A.Features.home.Domain.Repositories;
using AppFinalAv4A.Features.home.Infrastructure.Datasource;
using AppFinalAv4A.Features.home.Infrastructure.Repositories;
using AppFinalAv4A.Features.home.Presentation.Screens;
using AppFinalAv4A.Features.home.Presentation.ViewModels;
using AppFinalAv4A.Features.products.Presentation.Screens;
using AppFinalAv4A.Features.sales.Presentation.Screens;
using AppFinalAv4A.Features.Shared.Presentation.Screens;
using AppFinalAv4A.Features.Shared.Presentation.ViewModels;
namespace AppFinalAv4A;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder


	.UseMauiApp<App>()
 	.UseSkiaSharp()
	.AddPopups()
	.UseMauiCommunityToolkit()
	.ConfigureFonts(fonts =>
	{
		fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
		fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
		fonts.AddFont("MaterialSymbolsOutlined.ttf", "MaterialSymbols");
	});

#if DEBUG
		builder.Logging.AddDebug();
#endif
		builder.Services.AddSingleton<ILocalizationService>(LocalizationService.Instance);
		builder.Services.AddSingleton<HttpClient>();
		builder.Services.AddSingleton<TokenStorageService>();

		// API (backend Spring Boot)
		builder.Services.AddSingleton<IAccessTokenProvider, AccessTokenProvider>();
		builder.Services.AddSingleton<IApiClient, ApiClient>();

		builder.Services.AddSingleton<IAuthDataSource, AuthDataSourceImpl>();
		builder.Services.AddSingleton<IAuthRepository, AuthRepositoryImpl>();
		builder.Services.AddTransient<LoginViewModel>();
		builder.Services.AddTransient<LoginScreen>();
		builder.Services.AddTransient<ProfileViewModel>();
		builder.Services.AddTransient<ProfileScreen>();
		builder.Services.AddTransient<LanguageViewModel>();
		builder.Services.AddTransient<LanguageScreen>();

		// Home
		builder.Services.AddSingleton<IHomeDataSource, HomeDataSourceImpl>();
		builder.Services.AddSingleton<IHomeRepository, HomeRepositoryImpl>();
		builder.Services.AddTransient<HomeViewModel>();
		builder.Services.AddTransient<HomeScreen>();

		// Pestañas temporales y pantalla "próximamente"
		builder.Services.AddTransient<ProductsScreen>();
		builder.Services.AddTransient<SalesScreen>();
		builder.Services.AddTransient<ComingSoonViewModel>();
		builder.Services.AddTransient<ComingSoonScreen>();
		builder.Services.AddSingleton<
	IAppDialogService,
	AppDialogService>();
		return builder.Build();
	}
}
