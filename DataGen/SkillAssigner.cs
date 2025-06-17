using TalentFlow.Demo.Models;

namespace TalentFlow.Demo.DataGen;
public static class SkillAssigner
{
    private static readonly Random _random = new();
    public static void Assign()
    {
        var teamMembers = Generator.TeamMembers;
        foreach (var member in teamMembers)
        {
            // Each member will have between 4 and 13 random technical skills
            var numberOfSkills = _random.Next(4, 13);
            var memberSkills = Generator.UserSkills
                .OrderBy(x => _random.Next())
                .Take(numberOfSkills);

            member.UserSkills = [];

            foreach (var userSkill in memberSkills)
            {
                var skill = new UserSkill
                {
                    Name = userSkill.Name,
                    Complexity = userSkill.Complexity,
                    RetentionRate = Math.Round(_random.NextDouble() * (0.9 - 0.3) + 0.3, 2),
                    LastUse = DateTime.Now.AddDays(-_random.Next(0, 61)),
                };

                member.UserSkills.Add(skill);
            }
        }
        Generator.TeamMembers = teamMembers;
    }
}