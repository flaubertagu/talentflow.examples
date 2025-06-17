using TalentFlow.Csharp.Models.Enums;

namespace TalentFlow.Demo.Converters
{
    public static class EnumsConverter
    {
        public static string ComplexityToString(Complexity complexity)
        {
            return complexity switch
            {
                Complexity.Complex => "Complex",
                Complexity.Moderate => "Moderate",
                Complexity.Basic => "Basic",
                _ => "Basic"
            };
        }

        public static string AttributionChoiceToString(AttributionChoice attributionChoice)
        {
            return attributionChoice switch
            {
                AttributionChoice.All => "All",
                AttributionChoice.Best_performance => "Best performance",
                AttributionChoice.Best_relearning => "Best relearning",
                _ => "Best performance",
            };
        }

        public static string LearningSetToString(LearningSet learningSet)
        {
            return learningSet switch
            {
                LearningSet.Move_start_date_closer => "Move start date closer",
                LearningSet.Move_end_date_later => "Move end date later",
                _ => "Move end date later",
            };
        }
    }
}
