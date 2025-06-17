using TalentFlow.Demo.Models;
using TalentFlow.Csharp.Core.Helpers;
using TalentFlow.Csharp.Models.Objects;

namespace TalentFlow.Demo.Converters
{
    public class ConvertToAttributionSetting : IConverter<TaskSetting, AttributionSetting>
    {
        public AttributionSetting Convert(TaskSetting source)
        {
            return new AttributionSetting()
            {
                MaxOverBookTasks = source.MaxOverBookTasks,
                AttributionChoice = EnumsHelper.GetAttributionChoice(source.TypeOfAttribution),
                RecruitmentRequired = source.RecruitmentRequired,
                MinProductivityBasic = source.MinProductivityBasic,
                MinProductivityModerate = source.MinProductivityModerate,
                MinProductivityComplex = source.MinProductivityComplex,
                RelearningPercentagePerDay = source.RelearningPercentagePerDay,
                DeepLearningThreshold = source.DeepLearningThreshold,
                LearningSet = EnumsHelper.GetLearningSet(source.LearningType),
            };
        }

        public AttributionSetting? ConvertNull(TaskSetting? source)
        {
            if (source == null) return null;
            return Convert(source);
        }

        public List<AttributionSetting> Convert(List<TaskSetting> sourceList)
        {
            return sourceList?.Select(Convert).ToList() ?? [];
        }
    }
}
