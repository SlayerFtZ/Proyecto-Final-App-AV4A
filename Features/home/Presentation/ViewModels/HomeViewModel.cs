using System.Collections.ObjectModel;
using System.Globalization;
using AppFinalAv4A.Core.Localization;
using AppFinalAv4A.Core.Network;
using AppFinalAv4A.Core.Presentation;
using AppFinalAv4A.Features.auth.Domain.Entities;
using AppFinalAv4A.Features.auth.Domain.Repositories;
using AppFinalAv4A.Features.home.Domain.Entities;
using AppFinalAv4A.Features.home.Domain.Repositories;
using AppFinalAv4A.Features.Shared.Presentation.Screens;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AppFinalAv4A.Features.home.Presentation.ViewModels;

public partial class HomeViewModel : BaseViewModel
{
    const int CollapsedAlertCount = 3;
    static readonly CultureInfo MoneyCulture = new("es-MX");

    readonly IHomeRepository _homeRepository;
    readonly IAuthRepository _authRepository;

    int _loadInProgress;

    IReadOnlyList<AlertItem> _allAlerts = Array.Empty<AlertItem>();

    public HomeViewModel(
        IHomeRepository homeRepository,
        IAuthRepository authRepository,
        ILocalizationService localization)
        : base(localization)
    {
        _homeRepository = homeRepository;
        _authRepository = authRepository;
    }

    public ObservableCollection<AlertItem> Alerts { get; } = new();

    public ObservableCollection<TopSeller> TopSellers { get; } = new();

    [ObservableProperty] private string todayText = string.Empty;
    [ObservableProperty] private string greeting = string.Empty;
    [ObservableProperty] private string initials = string.Empty;
    [ObservableProperty] private string? pictureUrl;

    [ObservableProperty] private string revenueText = "$0.00";
    [ObservableProperty] private long salesCount;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasAlerts), nameof(HasNoAlerts), nameof(HasMoreAlerts))]
    private int alertsCount;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasNoAlerts), nameof(HasNoTopSellers))]
    private bool hasLoaded;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(SeeAllText))]
    private bool showAllAlerts;

    [ObservableProperty] private bool isRefreshing;

    public bool HasAlerts => AlertsCount > 0;
    public bool HasNoAlerts => HasLoaded && AlertsCount == 0;
    public bool HasMoreAlerts => AlertsCount > CollapsedAlertCount;
    public bool HasTopSellers => TopSellers.Count > 0;
    public bool HasNoTopSellers => HasLoaded && TopSellers.Count == 0;
    public string SeeAllText => ShowAllAlerts ? T("Show less") : T("See all");

    protected override string DefaultErrorMessage => T("Couldn't load today's summary.");

    [RelayCommand]
    private Task LoadAsync() => LoadSummaryAsync();

    [RelayCommand]
    private async Task RefreshAsync()
    {
        try
        {
            await LoadSummaryAsync();
        }
        finally
        {
            IsRefreshing = false;
        }
    }

    async Task LoadSummaryAsync()
    {
      
        if (Interlocked.CompareExchange(ref _loadInProgress, 1, 0) == 1)
            return;

        try
        {
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                IsBusy = !HasLoaded; 
                ErrorMessage = null;
            });

            var session = await _authRepository.GetCurrentSessionAsync().ConfigureAwait(false);
            if (session is null)
            {
                await GoToLoginAsync();
                return;
            }

    
            var summary = await _homeRepository.GetDaySummaryAsync().ConfigureAwait(false);

           
            await MainThread.InvokeOnMainThreadAsync(() => Apply(session, summary));
        }
        catch (ApiException ex) when (ex.IsUnauthorized)
        {
            await _authRepository.SingOutAsync();
            await GoToLoginAsync();
        }
        catch (Exception ex)
        {
            await MainThread.InvokeOnMainThreadAsync(() => ErrorMessage = ResolveErrorMessage(ex));
        }
        finally
        {
            await MainThread.InvokeOnMainThreadAsync(() => IsBusy = false);
            Interlocked.Exchange(ref _loadInProgress, 0);
        }
    }

    void Apply(AuthGoogle session, DaySummary summary)
    {
        var culture = Localization.CurrentCulture;
        var firstName = (session.Name ?? session.Email).Split(' ', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault()
                        ?? session.Email;

        TodayText = FormatToday(culture);
        Greeting = string.Format(T("Hello, {0}"), firstName);
        Initials = BuildInitials(session.Name ?? session.Email);
        PictureUrl = session.PictureProfileUrl;

        RevenueText = summary.Revenue.ToString("C2", MoneyCulture);
        SalesCount = summary.SalesCount;

        _allAlerts = summary.Alerts;
        AlertsCount = _allAlerts.Count;
        ApplyVisibleAlerts();

        TopSellers.ReplaceWith(summary.TopSellers);
        OnPropertyChanged(nameof(HasTopSellers));
        OnPropertyChanged(nameof(HasNoTopSellers));
        OnPropertyChanged(nameof(SeeAllText));

        HasLoaded = true;
    }

    void ApplyVisibleAlerts()
    {
        var visible = ShowAllAlerts ? _allAlerts : _allAlerts.Take(CollapsedAlertCount);
        Alerts.ReplaceWith(visible);
    }

    [RelayCommand]
    private void ToggleAlerts()
    {
        ShowAllAlerts = !ShowAllAlerts;
        ApplyVisibleAlerts();
    }

    [RelayCommand]
    private Task OpenNewSaleAsync() => Shell.Current.GoToAsync("//SalesScreen");

    [RelayCommand]
    private Task OpenProductsAsync() => Shell.Current.GoToAsync("//ProductsScreen");

    [RelayCommand]
    private Task OpenProfileAsync() => Shell.Current.GoToAsync("//ProfileScreen");

    [RelayCommand]
    private Task OpenScannerAsync()
        => Shell.Current.GoToAsync($"{nameof(ComingSoonScreen)}?feature=scanner");

    [RelayCommand]
    private Task OpenReportsAsync()
        => Shell.Current.GoToAsync($"{nameof(ComingSoonScreen)}?feature=reports");

    static Task GoToLoginAsync()
        => MainThread.InvokeOnMainThreadAsync(() => Shell.Current.GoToAsync("//LoginScreen"));

    static string FormatToday(CultureInfo culture)
    {
        var pattern = culture.TwoLetterISOLanguageName == "es" ? "dddd, d 'de' MMMM" : "dddd, MMMM d";
        var text = DateTime.Now.ToString(pattern, culture);
        return text.Length > 0 ? char.ToUpper(text[0], culture) + text[1..] : text;
    }

    static string BuildInitials(string text)
    {
        var parts = text.Split(new[] { ' ', '@', '.' }, StringSplitOptions.RemoveEmptyEntries);
        return parts.Length switch
        {
            0 => "?",
            1 => parts[0][..1].ToUpperInvariant(),
            _ => (parts[0][..1] + parts[1][..1]).ToUpperInvariant()
        };
    }
}
