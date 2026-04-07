using CampusNavigator.Services;

namespace CampusNavigator
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new AppShell();
        }

        protected override async void OnStart()
        {
            base.OnStart();

            // Daca nu exista profil salvat, trimitem la SetupPage
            if (!ProfileService.IsSetupDone())
            {
                await Shell.Current.GoToAsync("//SetupPage");
            }
            else
            {
                await Shell.Current.GoToAsync("//OrarPage");
            }
        }
    }
}