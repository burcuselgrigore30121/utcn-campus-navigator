namespace CampusNavigator.Models
{
    public class TaskItem
    {
        public int    Id       { get; set; }
        public string Text     { get; set; } = "";
        public string Subject  { get; set; } = "IRA";
        public string Priority { get; set; } = "Normal";
        public string Deadline { get; set; } = "";
        public bool   Done     { get; set; } = false;
        public int PriorityOrder => Priority switch { "Urgent"=>0, "Normal"=>1, _=>2 };
    }
}
