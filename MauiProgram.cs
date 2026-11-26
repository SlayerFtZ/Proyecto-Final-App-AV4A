using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using MPowerKit.Lottie;
using AppFinalAv4A.Features.auth.Domain.Datasource;
using AppFinalAv4A.Features.auth.Domain.Repositories;
using AppFinalAv4A.Features.auth.Infrastructure.Datasource;
using AppFinalAv4A.Features.auth.Infrastructure.Repositories;
using AppFinalAv4A.Features.auth.Service;
using AppFinalAv4A.Features.auth.Presentation.screens;


namespace AppFinalAv4A;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
	.UseMauiApp<App>()
	//Inicializar las librerias que intalamos para el proyecto
	.UseMauiCommunityToolkit()
	.UseMPowerKitLottie()
	.ConfigureFonts(fonts =>
	{
		fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
		fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
	});

#if DEBUG
		builder.Logging.AddDebug();
#endif
		builder.Services.AddSingleton<HttpClient>();
		builder.Services.AddSingleton<TokenStorageService>();
		builder.Services.AddSingleton<IAuthDataSource, AuthDataSourceImpl>();
		builder.Services.AddSingleton<IAuthRepository, AuthRepositoryImpl>();
		builder.Services.AddTransient<LoginViewModel>();
		builder.Services.AddTransient<LoginScreen>();
		return builder.Build();
	}
}
