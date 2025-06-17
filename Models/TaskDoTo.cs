namespace TalentFlow.Demo.Models
{
    public class TaskDoTo
    {
        public int Id { get; set; }
        public string TaskName { get; set; } = string.Empty;
        public string TaskDescription { get; set; } = string.Empty;
        public string AssignedTo { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<UserSkill> UserSkills { get; set; } = [];

        public string TaskComplexity { get; set; } = string.Empty;
        public double UserScore { get; set; }
        public double RequiredScore { get; set; }
        public bool OverBooked { get; set; }
        public double OverBookRatio { get; set; }
        public int Category { get; set; }
        public double AverageRRincrease { get; set; }
        public DateTime RelearningStartDate { get; set; }
        public DateTime RelearningEndDate { get; set; }
        public int RelearningDuration { get; set; }
    }
}
