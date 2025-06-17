using TalentFlow.Demo.Models;
using TalentFlow.Csharp.Core.Helpers;
using TalentFlow.Csharp.Models.Objects;

namespace TalentFlow.Demo.Converters
{
    public class ConvertToAIStrategicSkill : IConverter<StrategicSkill, AIStrategicSkill>
    {
        public AIStrategicSkill Convert(StrategicSkill source)
        {
            return new AIStrategicSkill()
            {
                SkillName = source.Equivalent, //Use Equivalent to match the skill register in your system
                SkillComplexity = EnumsHelper.GetComplexity(source.Complexity),
                ComplementarySkills = source.ComplementarySkills,
            };
        }

        public AIStrategicSkill? ConvertNull(StrategicSkill? source)
        {
            if (source == null) return null;
            return Convert(source);
        }

        public List<AIStrategicSkill> Convert(List<StrategicSkill> sourceList)
        {
            return sourceList?.Select(Convert).ToList() ?? [];
        }
    }
}
