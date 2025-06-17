using TalentFlow.Demo.Models;
using Microsoft.AspNetCore.Components;
using TalentFlow.Csharp.Models.Enums;
using TalentFlow.Csharp.Models.Objects;

namespace TalentFlow.Demo.Components.Shared
{
    public partial class ActionPlanResultVisualizer
    {
        [Parameter]
        public MyStrategy? StrategySelected { get; set; }
        [Parameter]
        public StrategicSkillResult? GlobalResult { get; set; }
        [Parameter]
        public List<StrategicSkillResult> SkillResults { get; set; } = [];
        [Parameter]
        public List<StrategicSkillNeed> ActionPlanSkills { get; set; } = [];
        [Parameter]
        public AIActionPlan? ActionPlanResult { get; set; }
        [Parameter]
        public List<StrategyCandidates> Candidates { get; set; } = [];
        [Parameter]
        public DateTime OptimalDate { get; set; }
        [Parameter]
        public bool DisplayOptimalDate { get; set; }
        [Parameter]
        public EventCallback RefreshStrategies { get; set; }
        [Parameter]
        public double MinimumScore { get; set; }

        public IsBusy IsBusy { get; set; } = new();

        #region Notions
        private string _notion1 = "Ratio of employees using at least one of the required technical skills";
        private string _notion2 = "Maximum productivity rate if the strategy were implemented today";
        private string _notion3 = "Evaluation of the success rate of the strategy if no recommendations are taken into account";
        #endregion

        #region Recommandation
        private string _preRecommandation = "To increase the chances of success of your strategy, you must:";
        private string _ratio1 = "Recruit qualified personnel now, and in large numbers if possible";
        private string _ratio2 = "Recruit qualified staff or train new employees to increase task assignment flexibility";
        private string _ratio3 = "Train new employees to increase task assignment flexibility";
        private string _ratio4 = "Monitor the potential decline in the number of employees with the required skills";
        private string _productivity1 = "Train all employees capable of mastering the required skills now";
        private string _productivity2 = "Train employees who already have the skills required to improve productivity";
        private string _productivity3 = "Train employees whose skills are very lacking";
        private string _productivity4 = "Monitor the decline in productivity of employees with the required skills";

        private string RatioAdvice(int rating)
        {
            switch (rating)
            {
                case 1: return _ratio1;
                case 2: return _ratio2;
                case 3: return _ratio3;
                case 4: return _ratio4;
                default: return _ratio1;
            }
        }

        private string ProductivityAdvice(int rating)
        {
            switch (rating)
            {
                case 1: return _productivity1;
                case 2: return _productivity2;
                case 3: return _productivity3;
                case 4: return _productivity4;
                default: return _productivity1;
            }
        }
        #endregion

        #region ActionPlan
        private bool _expandedStrategicSkills = false;
        private void ShowStrategicSkillsTable() => _expandedStrategicSkills = !_expandedStrategicSkills;
        private bool RecommandationOutlined(RecommendedStrategy type)
        {
            if (nameof(type) == ActionPlanResult!.RecommandationCost) return true;
            return false;
        }
        #endregion
    }
}
