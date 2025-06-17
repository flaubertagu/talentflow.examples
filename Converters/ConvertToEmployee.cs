using TalentFlow.Demo.Models;
using TalentFlow.Csharp.Core.Helpers;
using TalentFlow.Csharp.Models.Objects;

namespace TalentFlow.Demo.Converters
{
    public class ConvertToEmployee : IConverter<TeamMember, Employee>
    {
        public Employee Convert(TeamMember source)
        {
            return new Employee()
            {
                EmployeeName = source.Name,
                EmployeSkills = source.UserSkills.Select(skill => new EmployeeSkill()
                {
                    Id = source.Id,
                    EmployeeName = source.Name,
                    SkillName = skill.Name,
                    SkillComplexity = EnumsHelper.GetComplexity(skill.Complexity),
                    RetentionRate = skill.RetentionRate,
                    LastUse = skill.LastUse,
                }).ToList(),
            };
        }

        public Employee? ConvertNull(TeamMember? source)
        {
            if (source == null) return null;
            return Convert(source);
        }

        public List<Employee> Convert(List<TeamMember> sourceList)
        {
            return sourceList?.Select(Convert).ToList() ?? [];
        }
    }
}
