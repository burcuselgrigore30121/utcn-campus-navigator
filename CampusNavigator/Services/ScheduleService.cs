using System.Text.Json;
using CampusNavigator.Models;

namespace CampusNavigator.Services
{
    public class ScheduleService
    {
        private List<ScheduleEntry>? _allEntries;

        public async Task<List<ScheduleEntry>> LoadAllAsync()
        {
            if (_allEntries != null)
                return _allEntries;

            // schedule.json e inclus ca MauiAsset in Resources/Raw/
            using var stream = await FileSystem.OpenAppPackageFileAsync("schedule.json");
            using var reader = new StreamReader(stream);
            var json = await reader.ReadToEndAsync();
            _allEntries = JsonSerializer.Deserialize<List<ScheduleEntry>>(json) ?? new();
            return _allEntries;
        }

        /// <summary>
        /// Returneaza activitatile pentru ziua data, filtrate dupa profil si paritate saptamana.
        /// </summary>
        public async Task<List<ScheduleEntry>> GetForDayAsync(
            UserProfile profile,
            string zi,
            bool isSaptamanaImpara)
        {
            var all = await LoadAllAsync();

            return all
                .Where(e =>
                    e.Specializare == profile.Specializare &&
                    e.An == profile.An &&
                    e.Limba == profile.Limba &&
                    e.Grupa == profile.Grupa &&
                    e.Semigrupa == profile.Semigrupa &&
                    e.Zi == zi &&
                    e.IsActiveForWeek(isSaptamanaImpara))
                .OrderBy(e => e.StartHour)
                .ToList();
        }

        /// <summary>
        /// Returneaza toate activitatile saptamanii (Luni-Vineri).
        /// </summary>
        public async Task<Dictionary<string, List<ScheduleEntry>>> GetWeekAsync(
            UserProfile profile,
            bool isSaptamanaImpara)
        {
            var zile = new[] { "Luni", "Marti", "Miercuri", "Joi", "Vineri" };
            var result = new Dictionary<string, List<ScheduleEntry>>();

            foreach (var zi in zile)
            {
                result[zi] = await GetForDayAsync(profile, zi, isSaptamanaImpara);
            }

            return result;
        }

        /// <summary>
        /// Returneaza lista de grupe disponibile pentru specializare+an+limba.
        /// </summary>
        public async Task<List<int>> GetGrupeAsync(string specializare, int an, string limba)
        {
            var all = await LoadAllAsync();
            return all
                .Where(e => e.Specializare == specializare && e.An == an && e.Limba == limba)
                .Select(e => e.Grupa)
                .Distinct()
                .OrderBy(g => g)
                .ToList();
        }
    }
}
