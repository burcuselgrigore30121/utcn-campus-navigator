using System.Text.RegularExpressions;

namespace CampusNavigator.Services
{
    public class AnnouncementEntry
    {
        public string Title { get; set; } = "";
        public string Date  { get; set; } = "";
        public string Url   { get; set; } = "";
    }

    public class AnnouncementService
    {
        private static readonly HttpClient _http = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(10)
        };

        private List<AnnouncementEntry>? _cached;
        private DateTime _lastFetch = DateTime.MinValue;

        public async Task<List<AnnouncementEntry>> GetAnnouncementsAsync(bool forceRefresh = false)
        {
            // Cache 10 minute
            if (_cached != null && !forceRefresh && (DateTime.Now - _lastFetch).TotalMinutes < 10)
                return _cached;

            try
            {
                var html = await _http.GetStringAsync("https://ac.utcluj.ro/anunturi.html");
                _cached = ParseAnnouncements(html);
                _lastFetch = DateTime.Now;
                return _cached;
            }
            catch
            {
                // Fallback daca nu e internet
                return _cached ?? GetFallback();
            }
        }

        private List<AnnouncementEntry> ParseAnnouncements(string html)
        {
            var results = new List<AnnouncementEntry>();

            // Pattern: data + titlu + link
            // Format in HTML: "DD-MM-YYYY HH:MM\n\n[Titlu](link)"
            var pattern = @"(\d{2}-\d{2}-\d{4})\s+\d{2}:\d{2}\s*<br\s*/?>\s*<a\s+href=""([^""]+)""\s+title=""[^""]*"">([^<]+)<\/a>";
            var matches = Regex.Matches(html, pattern, RegexOptions.IgnoreCase);

            foreach (Match m in matches.Take(20))
            {
                var url = m.Groups[2].Value;
                if (!url.StartsWith("http")) url = "https://ac.utcluj.ro" + url;
                results.Add(new AnnouncementEntry
                {
                    Date  = m.Groups[1].Value,
                    Url   = url,
                    Title = System.Net.WebUtility.HtmlDecode(m.Groups[3].Value.Trim()),
                });
            }

            // Fallback regex mai simplu daca primul nu gaseste nimic
            if (results.Count == 0)
            {
                var pattern2 = @"href=""(/anunt/[^""]+)""\s+title=""[^""]*"">([^<]{10,})</a>";
                var m2 = Regex.Matches(html, pattern2, RegexOptions.IgnoreCase);
                // Gaseste datele separat
                var dates = Regex.Matches(html, @"\d{2}-\d{2}-\d{4}");
                int di = 0;
                foreach (Match m in m2.Take(20))
                {
                    results.Add(new AnnouncementEntry
                    {
                        Date  = di < dates.Count ? dates[di++].Value : "",
                        Url   = "https://ac.utcluj.ro" + m.Groups[1].Value,
                        Title = System.Net.WebUtility.HtmlDecode(m.Groups[2].Value.Trim()),
                    });
                }
            }

            return results;
        }

        private List<AnnouncementEntry> GetFallback() => new()
        {
            new() { Date="06-04-2026", Title="Rezultatele Concursului de programare pentru elevii de liceu - editia 2026", Url="https://ac.utcluj.ro/anunt/rezultatele-concursului-de-programare-pentru-elevii-de-liceu-editia-2026.html" },
            new() { Date="31-03-2026", Title="Burse 2025-2026, semestrul 2 — Medii minime actualizate", Url="https://ac.utcluj.ro/anunt/burse-2025-2026-semestrul-2-2.html" },
            new() { Date="30-03-2026", Title="Premierea Concursului D'AIA 2026", Url="https://ac.utcluj.ro/anunt/premierea-concursului-d-aia-2026.html" },
            new() { Date="24-03-2026", Title="Sesiune excepțională ani terminali", Url="https://ac.utcluj.ro/anunt/sesiune-exceptionala-ani-terminali-2.html" },
            new() { Date="16-03-2026", Title="BEST recrutează!", Url="https://ac.utcluj.ro/anunt/best-recruteaza.html" },
        };
    }
}
