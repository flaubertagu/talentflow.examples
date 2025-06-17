using TalentFlow.Demo.Models;
using TalentFlow.Csharp.Models.Objects;

namespace TalentFlow.Demo.Converters
{
    public class ConvertToAISkill : IConverter<Skill, AISkill>
    {
        public AISkill Convert(Skill source)
        {
            return new()
            {
                Name = source.Name,
                Complexity = source.Complexity,
            };
        }

        public AISkill? ConvertNull(Skill? source)
        {
            if (source == null) return null;
            return Convert(source);
        }

        public List<AISkill> Convert(List<Skill> sourceList)
        {
            return sourceList?.Select(Convert).ToList() ?? [];
        }
    }
}
