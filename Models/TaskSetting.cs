using TalentFlow.Demo.Converters;
using TalentFlow.Csharp.Models.Enums;

namespace TalentFlow.Demo.Models
{
    public class TaskSetting
    {
        public int MaxOverBookTasks { get; set; } = 5;
        public string TypeOfAttribution { get; set; } = EnumsConverter.AttributionChoiceToString(AttributionChoice.Best_performance);
        public bool RecruitmentRequired { get; set; } = false;
        public double MinProductivityBasic { get; set; } = 90;
        public double MinProductivityModerate { get; set; } = 85;
        public double MinProductivityComplex { get; set; } = 80;
        public double RelearningPercentagePerDay { get; set; } = 80;
        public double DeepLearningThreshold { get; set; } = 80;
        public string LearningType { get; set; } = @EnumsConverter.LearningSetToString(LearningSet.Move_end_date_later);
    }
}
