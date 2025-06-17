namespace TalentFlow.Demo.Models
{
    public class UserSkill
    {
        public int Id { get; set; }
        public string MemberName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Complexity { get; set; } = string.Empty;
        public double RetentionRate { get; set; }
        public DateTime LastUse { get; set; }
        public int RelearningDuration { get; set; }
    }
}
