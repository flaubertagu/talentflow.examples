namespace TalentFlow.Demo.Models
{
    public class TeamMember
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<UserSkill> UserSkills { get; set; } = [];
    }
}
