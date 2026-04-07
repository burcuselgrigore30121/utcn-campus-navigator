using CampusNavigator.Models;
using CampusNavigator.Services;

namespace CampusNavigator.Views
{
    public partial class OrarPage : ContentPage
    {
        private readonly ScheduleService _scheduleService;
        private UserProfile? _profile;
        private bool _isSaptamanaImpara;
        private string _selectedZi = "Luni";
        private readonly string[] _zile = { "Luni", "Marți", "Miercuri", "Joi", "Vineri" };
        // mapam afisare -> cheie JSON
        private readonly Dictionary<string, string> _ziMap = new()
        {
            {"Luni","Luni"},{"Marți","Marti"},{"Miercuri","Miercuri"},{"Joi","Joi"},{"Vineri","Vineri"}
        };

        public OrarPage(ScheduleService scheduleService)
        {
            InitializeComponent();
            _scheduleService = scheduleService;
            _isSaptamanaImpara = UserProfile.IsCurrentWeekOdd();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            _profile = ProfileService.Load();

            if (_profile == null)
            {
                await Shell.Current.GoToAsync("//SetupPage");
                return;
            }

            BuildDayTabs();
            // selectam ziua curenta
            var today = DateTime.Today.DayOfWeek;
            _selectedZi = today switch
            {
                DayOfWeek.Monday => "Luni",
                DayOfWeek.Tuesday => "Marti",
                DayOfWeek.Wednesday => "Miercuri",
                DayOfWeek.Thursday => "Joi",
                DayOfWeek.Friday => "Vineri",
                _ => "Luni"
            };

            UpdateHeader();
            await LoadDayAsync();
        }

        private void BuildDayTabs()
        {
            ZileTabs.Children.Clear();
            foreach (var zi in _zile)
            {
                var border = new Border
                {
                    StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = 20 },
                    Padding = new Thickness(16, 8),
                    BackgroundColor = _ziMap[zi] == _selectedZi
                        ? Color.FromArgb("#3B6FD4")
                        : Colors.White,
                    Stroke = Colors.Transparent,
                };
                var label = new Label
                {
                    Text = zi,
                    FontSize = 13,
                    FontAttributes = _ziMap[zi] == _selectedZi ? FontAttributes.Bold : FontAttributes.None,
                    TextColor = _ziMap[zi] == _selectedZi ? Colors.White : Color.FromArgb("#555"),
                    VerticalOptions = LayoutOptions.Center,
                };
                border.Content = label;
                var tap = new TapGestureRecognizer();
                var capturedZi = _ziMap[zi];
                tap.Tapped += async (s, e) =>
                {
                    _selectedZi = capturedZi;
                    BuildDayTabs();
                    await LoadDayAsync();
                };
                border.GestureRecognizers.Add(tap);
                ZileTabs.Children.Add(border);
            }
        }

        private void UpdateHeader()
        {
            if (_profile == null) return;
            LblGrupa.Text = $"{_profile.Specializare} • An {_profile.An} • Gr. {_profile.Grupa} • Sg. {_profile.Semigrupa}";
            LblSaptamana.Text = _isSaptamanaImpara ? "Săptămână impară" : "Săptămână pară";

            // Update toggle buttons
            BtnImpara.BackgroundColor = _isSaptamanaImpara ? Color.FromArgb("#3B6FD4") : Colors.Transparent;
            ((Label)BtnImpara.Content).TextColor = _isSaptamanaImpara ? Colors.White : Color.FromArgb("#3B6FD4");
            ((Label)BtnImpara.Content).FontAttributes = _isSaptamanaImpara ? FontAttributes.Bold : FontAttributes.None;

            BtnPara.BackgroundColor = !_isSaptamanaImpara ? Color.FromArgb("#3B6FD4") : Colors.Transparent;
            ((Label)BtnPara.Content).TextColor = !_isSaptamanaImpara ? Colors.White : Color.FromArgb("#3B6FD4");
            ((Label)BtnPara.Content).FontAttributes = !_isSaptamanaImpara ? FontAttributes.Bold : FontAttributes.None;
        }

        private async Task LoadDayAsync()
        {
            if (_profile == null) return;
            var entries = await _scheduleService.GetForDayAsync(_profile, _selectedZi, _isSaptamanaImpara);
            OrarList.ItemsSource = entries;
        }

        private async void OnImparaTapped(object? sender, TappedEventArgs e)
        {
            _isSaptamanaImpara = true;
            UpdateHeader();
            await LoadDayAsync();
        }

        private async void OnParaTapped(object? sender, TappedEventArgs e)
        {
            _isSaptamanaImpara = false;
            UpdateHeader();
            await LoadDayAsync();
        }

        private async void OnMapsButtonTapped(object? sender, TappedEventArgs e)
        {
            if (e.Parameter is string url && !string.IsNullOrEmpty(url))
            {
                try
                {
                    await Launcher.OpenAsync(new Uri(url));
                }
                catch
                {
                    await DisplayAlert("Eroare", "Nu s-a putut deschide Google Maps.", "OK");
                }
            }
        }
    }
}
