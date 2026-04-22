using Microsoft.AspNetCore.Components.WebView.Maui;

namespace CampusNavigator
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            var blazorWebView = new BlazorWebView
            {
                HostPage = "wwwroot/index.html"
            };

            var routesType = typeof(App).Assembly
                .GetType("CampusNavigator.Components.Routes");

            blazorWebView.RootComponents.Add(new RootComponent
            {
                Selector = "#app",
                ComponentType = routesType!
            });

            Content = blazorWebView;
        }
    }
}
