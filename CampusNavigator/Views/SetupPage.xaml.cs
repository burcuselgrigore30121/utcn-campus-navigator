using CampusNavigator.Models;
using CampusNavigator.Services;

namespace CampusNavigator.Views
{
    public partial class SetupPage : ContentPage
    {
        private readonly ScheduleService _scheduleService;
        private int _selectedSemigrupa = 1;
        private List<int> _grupeDisponibile = new();

        public SetupPage(ScheduleService scheduleService)
        {
            InitializeComponent();
            _scheduleService = scheduleService;
        }

        private async void RefreshGrupe()
        {
            if (PickerSpecializare.SelectedIndex < 0 ||
                PickerAn.SelectedIndex < 0 ||
                PickerLimba.SelectedIndex < 0)
                return;

            var specializare = PickerSpecializare.SelectedIndex == 0 ? "Calculatoare" : "Automatica";
            var an = PickerAn.SelectedIndex + 1;
            var limba = PickerLimba.SelectedIndex == 0 ? "romana" : "engleza";

            _grupeDisponibile = await _scheduleService.GetGrupeAsync(specializare, an, limba);

            PickerGrupa.Items.Clear();
            foreach (var g in _grupeDisponibile)
                PickerGrupa.Items.Add(g.ToString());

            PickerGrupa.IsEnabled = _grupeDisponibile.Count > 0;
            PickerGrupa.SelectedIndex = -1;
            UpdateSaveButton();
        }

        private void OnSpecializareChanged(object? sender, EventArgs e) => RefreshGrupe();
        private void OnAnChanged(object? sender, EventArgs e) => RefreshGrupe();
        private void OnLimbaChanged(object? sender, EventArgs e) => RefreshGrupe();
        private void OnGrupaChanged(object? sender, EventArgs e) => UpdateSaveButton();

        private void OnSemigrupa1Tapped(object? sender, TappedEventArgs e)
        {
            _selectedSemigrupa = 1;
            BtnSg1.BackgroundColor = Color.FromArgb("#3B6FD4");
            BtnSg1.Stroke = Color.FromArgb("#3B6FD4");
            ((Label)BtnSg1.Content).TextColor = Colors.White;
            BtnSg2.BackgroundColor = Colors.White;
            BtnSg2.Stroke = Color.FromArgb("#DDE3EE");
            ((Label)BtnSg2.Content).TextColor = Color.FromArgb("#1A1A2E");
        }

        private void OnSemigrupa2Tapped(object? sender, TappedEventArgs e)
        {
            _selectedSemigrupa = 2;
            BtnSg2.BackgroundColor = Color.FromArgb("#3B6FD4");
            BtnSg2.Stroke = Color.FromArgb("#3B6FD4");
            ((Label)BtnSg2.Content).TextColor = Colors.White;
            BtnSg1.BackgroundColor = Colors.White;
            BtnSg1.Stroke = Color.FromArgb("#DDE3EE");
            ((Label)BtnSg1.Content).TextColor = Color.FromArgb("#1A1A2E");
        }

        private void UpdateSaveButton()
        {
            BtnSave.IsEnabled = PickerGrupa.SelectedIndex >= 0;
        }

        private async void OnSaveClicked(object? sender, EventArgs e)
        {
            var profile = new UserProfile
            {
                Specializare = PickerSpecializare.SelectedIndex == 0 ? "Calculatoare" : "Automatica",
                An = PickerAn.SelectedIndex + 1,
                Limba = PickerLimba.SelectedIndex == 0 ? "romana" : "engleza",
                Grupa = _grupeDisponibile[PickerGrupa.SelectedIndex],
                Semigrupa = _selectedSemigrupa,
            };

            ProfileService.Save(profile);
            await Shell.Current.GoToAsync("//OrarPage");
        }
    }
}
