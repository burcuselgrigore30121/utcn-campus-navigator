namespace CampusNavigator.Models
{
    public class UserProfile
    {
        public string Specializare { get; set; } = ""; // "Calculatoare" sau "Automatica"
        public int An { get; set; } = 1;
        public string Limba { get; set; } = "romana";  // "romana" sau "engleza"
        public int Grupa { get; set; }
        public int Semigrupa { get; set; } = 1;

        // Data de inceput a semestrului (folosita pentru calcul sapt para/impara)
        // Semestrul 2, 2025-2026 incepe pe 3 Februarie 2025 (saptamana impara)
        public static readonly DateTime SemesterStart = new DateTime(2025, 2, 3);

        public static bool IsCurrentWeekOdd()
        {
            int weekNumber = (int)((DateTime.Today - SemesterStart).TotalDays / 7) + 1;
            return weekNumber % 2 != 0;
        }
    }
}
