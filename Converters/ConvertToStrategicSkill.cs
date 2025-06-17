using TalentFlow.Demo.Models;
using TalentFlow.Csharp.Models.Objects;

namespace TalentFlow.Demo.Converters
{
    public class ConvertToStrategicSkill : IConverter<AIStrategicSkill, StrategicSkill>
    {
        public StrategicSkill Convert(AIStrategicSkill source)
        {
            return new StrategicSkill()
            {
                NameAI = source.SkillName,
                Equivalent = source.Equivalent,
                Complexity = EnumsConverter.ComplexityToString(source.SkillComplexity),
                ComplementarySkills = source.ComplementarySkills,
            };
        }

        public StrategicSkill? ConvertNull(AIStrategicSkill? source)
        {
            if (source == null) return null;
            return Convert(source);
        }

        public List<StrategicSkill> Convert(List<AIStrategicSkill> sourceList)
        {
            return sourceList?.Select(Convert).ToList() ?? [];
        }
    }
}
