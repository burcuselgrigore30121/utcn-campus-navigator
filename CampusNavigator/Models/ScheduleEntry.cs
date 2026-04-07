using System.Text.Json.Serialization;

namespace CampusNavigator.Models
{
    public class ScheduleEntry
    {
        [JsonPropertyName("specializare")]
        public string Specializare { get; set; } = "";

        [JsonPropertyName("an")]
        public int An { get; set; }

        [JsonPropertyName("limba")]
        public string Limba { get; set; } = "";

        [JsonPropertyName("grupa")]
        public int Grupa { get; set; }

        [JsonPropertyName("semigrupa")]
        public int Semigrupa { get; set; }

        [JsonPropertyName("zi")]
        public string Zi { get; set; } = "";

        [JsonPropertyName("slot")]
        public string Slot { get; set; } = "";

        [JsonPropertyName("saptamana")]
        public string Saptamana { get; set; } = ""; // "para", "impara", "ambele"

        [JsonPropertyName("materie")]
        public string Materie { get; set; } = "";

        [JsonPropertyName("tip")]
        public string? Tip { get; set; } // "Curs", "Seminar", "Laborator"

        [JsonPropertyName("sala")]
        public string? Sala { get; set; }

        [JsonPropertyName("maps")]
        public string? MapsUrl { get; set; }

        [JsonPropertyName("profesor")]
        public string? Profesor { get; set; }

        // ora de inceput ca int pentru sortare (ex: "8-10" -> 8, "14-16" -> 14)
        public int StartHour
        {
            get
            {
                var parts = Slot.Split('-');
                return int.TryParse(parts[0], out int h) ? h : 0;
            }
        }

        public bool IsActiveForWeek(bool isSaptamanaImpara)
        {
            return Saptamana == "ambele"
                || (isSaptamanaImpara && Saptamana == "impara")
                || (!isSaptamanaImpara && Saptamana == "para");
        }
    }
}
