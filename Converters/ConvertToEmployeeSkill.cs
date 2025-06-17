using TalentFlow.Demo.Models;
using TalentFlow.Csharp.Core.Helpers;
using TalentFlow.Csharp.Models.Objects;

namespace TalentFlow.Demo.Converters
{
    public class ConvertToEmployeeSkill : IConverter<UserSkill, EmployeeSkill>
    {
        public EmployeeSkill Convert(UserSkill source)
        {
            return new EmployeeSkill()
            {
                Id = source.Id,
                EmployeeName = source.MemberName,
                SkillName = source.Name,
                SkillComplexity = EnumsHelper.GetComplexity(source.Complexity),
                RetentionRate = source.RetentionRate,
                LastUse = source.LastUse,
            };
        }

        public EmployeeSkill? ConvertNull(UserSkill? source)
        {
            if (source == null) return null;
            return Convert(source);
        }

        public List<EmployeeSkill> Convert(List<UserSkill> sourceList)
        {
            return sourceList?.Select(Convert).ToList() ?? [];
        }
    }
}
