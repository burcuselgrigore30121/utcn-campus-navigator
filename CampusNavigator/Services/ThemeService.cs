namespace CampusNavigator.Services
{
    /// <summary>
    /// Singleton care tine tema (dark/light) sincronizata intre pagini.
    /// </summary>
    public class ThemeService
    {
        private bool _isDark;

        public event EventHandler<bool>? ThemeChanged;

        public bool IsDark
        {
            get => _isDark;
            set
            {
                if (_isDark == value) return;
                _isDark = value;
                ThemeChanged?.Invoke(this, value);
            }
        }

        public ThemeService()
        {
            _isDark = Application.Current?.RequestedTheme == AppTheme.Dark;
            if (Application.Current != null)
                Application.Current.RequestedThemeChanged += (s, e) =>
                    IsDark = e.RequestedTheme == AppTheme.Dark;
        }

        // Palette
        public Color Bg      => _isDark ? Color.FromArgb("#0D0D1A") : Color.FromArgb("#F0EEFF");
        public Color CardBg  => _isDark ? Color.FromArgb("#161626") : Colors.White;
        public Color TxtMain => _isDark ? Color.FromArgb("#E2E8F0") : Color.FromArgb("#1A1535");
        public Color TxtSub  => _isDark ? Color.FromArgb("#6B7DB3") : Color.FromArgb("#5B5280");
        public Color Pill    => _isDark ? Color.FromArgb("#1A1A2E") : Color.FromArgb("#EDE8FF");
        public Color NavBg   => _isDark ? Color.FromArgb("#0A0814") : Color.FromArgb("#F8F6FF");
        public Color Inact   => _isDark ? Color.FromArgb("#3A3A5A") : Color.FromArgb("#B0A8D0");
        public Color ThemeBg => _isDark ? Color.FromArgb("#1E1E3A") : Color.FromArgb("#FFF8E7");
        public Color Sep     => _isDark ? Color.FromArgb("#FFFFFF") : Color.FromArgb("#1A1535");
        public Color PickerBg=> _isDark ? Color.FromArgb("#1E1E3A") : Color.FromArgb("#F0ECFF");

        public LinearGradientBrush AccentGrad => new(
            new GradientStopCollection {
                new GradientStop(Color.FromArgb("#7C6FFF"), 0),
                new GradientStop(Color.FromArgb("#A78BFA"), 1)
            }, new Point(0,0), new Point(1,1));

        public LinearGradientBrush WeekGrad => new(
            new GradientStopCollection {
                new GradientStop(Color.FromArgb("#FF6B6B"), 0),
                new GradientStop(Color.FromArgb("#FFB347"), 1)
            }, new Point(0,0), new Point(1,0));

        public LinearGradientBrush NavActiveGrad => new(
            new GradientStopCollection {
                new GradientStop(Color.FromArgb("#C3A6FF"), 0),
                new GradientStop(Color.FromArgb("#A78BFA"), 1)
            }, new Point(0,0), new Point(1,1));
    }
}
