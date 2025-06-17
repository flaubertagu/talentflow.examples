namespace TalentFlow.Demo.Models
{
    public class TaskCompleted
    {
        public required int Id { get; set; }
        public required string TaskName { get; set; }
        public required string TaskDescription { get; set; }
        public required string AssignedTo { get; set; }
        public required DateTime StartDate { get; set; }
        public required DateTime EndDate { get; set; }
        public required DateTime DeliveryDate { get; set; }
        public required List<Skill> TaskSkills { get; set; } = [];
        public int Occurence { get; set; } = 1;
    }
}
