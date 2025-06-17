using TalentFlow.Demo.Models;
using TalentFlow.Csharp.Models.Objects;

namespace TalentFlow.Demo.Converters
{
    public class ConvertToUserSkill : IConverter<EmployeeSkill, UserSkill>
    {
        public UserSkill Convert(EmployeeSkill source)
        {
            return new UserSkill
            {
                Id = source.Id,
                MemberName = source.EmployeeName,
                Name = source.SkillName,
                Complexity = nameof(source.SkillComplexity),
                RetentionRate = source.RetentionRate,
                LastUse = source.LastUse,
            };
        }

        public UserSkill? ConvertNull(EmployeeSkill? source)
        {
            if (source == null) return null;
            return Convert(source);
        }

        public List<UserSkill> Convert(List<EmployeeSkill> sourceList)
        {
            return sourceList?.Select(Convert).ToList() ?? [];
        }
    }
}
