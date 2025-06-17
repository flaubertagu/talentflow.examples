namespace TalentFlow.Demo.Models
{
    public class MyStrategy
    {
        public string ActivityArea { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Strategy { get; set; } = string.Empty;
        public List<StrategicSkill> StrategicSkills { get; set; } = [];
    }

    public class StrategicSkill
    {
        public string NameAI { get; set; } = string.Empty;
        public string Equivalent { get; set; } = string.Empty;
        public string Complexity { get; set; } = string.Empty;
        public List<string> ComplementarySkills { get; set; } = [];
    }
}
