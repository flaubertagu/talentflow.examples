using TalentFlow.Demo.Components.Shared;
using TalentFlow.Demo.DataGen;
using TalentFlow.Demo.Models;
using MudBlazor;
using TalentFlow.Csharp.Core.Helpers;
using TalentFlow.Csharp.Models.Enums;
using TalentFlow.Csharp.Models.Objects;

namespace TalentFlow.Demo.Components.Pages
{
    public partial class Action_Plan
    {
        /// <summary>
        /// Allows you to retrieve the configuration status of your OpenAI settings in TalentFlow.Csharp.AI 
        /// </summary>
        #region TalentFlow.Csharp.AI status configuration
        public bool SemanticIsConfigured { get; set; }
        public void AiConfigResult(bool config) => SemanticIsConfigured = config; 
        #endregion

        public IsBusy IsBusy { get; set; } = new();
        private List<Skill> Skills { get; set; } = [];
        public List<MyStrategy> Strategies { get; set; } = [];
        private List<TeamMember> TeamMembers { get; set; } = [];
        public List<TaskCompleted> TasksCompleted { get; set; } = [];
        private StrategicSkillResult? GlobalResult { get; set; }
        private List<StrategicSkillResult> SkillResults { get; set; } = [];

        protected override async Task OnInitializedAsync()
        {
            await GenerateData();
        }

        private Task GenerateData()
        {
            Generator.GenerateTaskCompleted();
            Skills = Generator.Skills;
            Strategies = Generator.Strategies;
            TeamMembers = Generator.TeamMembers;
            TasksCompleted = Generator.TasksCompleted;
            return Task.CompletedTask;
        }

        #region Dates
        private DateTime OptimalDate { get; set; } = DateTime.MinValue;

        private DateTime? DateChoosed = DateTime.Today;
        private List<DateTime> DatesInRange { get; set; } = [];
        private DateRange DatesRange { get; set; } = new DateRange() { Start = DateTime.Today, End = DateTime.Today.AddMonths(1).AddDays(-1) };
        #endregion

        #region Action plan
        private MyStrategy? StrategySelected { get; set; }
        private bool AutomateAll { get; set; } = true;
        private bool SingleAnalysisMade { get; set; } = false;
        private void OnAnalysisTypeChange(bool value) => AutomateAll = value;
        public List<StrategicSkillNeed> ActionPlanSkills { get; set; } = [];
        public AIActionPlan? ActionPlanResult { get; set; }
        private List<StrategyCandidates> Candidates { get; set; } = [];
        private bool DisplayOptimalDate => GlobalResult?.GlobalNote > 0 && AutomateAll && !SingleAnalysisMade && OptimalDate != DateTime.MinValue;
        private double MinimumScore { get; set; } = 0.2;

        private void ResetAnalysis()
        {
            SkillResults.Clear();
            GlobalResult = null;
        }

        private void ResetPlanGenerated()
        {
            ActionPlanResult = null;
            ActionPlanSkills.Clear();
            Candidates.Clear();
            ActionPlanSkills.Clear();
        }

        private void ResetSelection()
        {
            ResetActionPlanResult();
            StrategySelected = null;
            DateChoosed = DateTime.Today;
            DatesRange = new DateRange() { Start = DateTime.Today, End = DateTime.Today.AddMonths(1).AddDays(-1) };
        }

        private void ResetActionPlanResult()
        {
            ResetAnalysis();
            ResetPlanGenerated();
            StateHasChanged();
        }

        private async Task LaunchAnalysis()
        {
            await IsBusy.Loading();
            try
            {
                if (StrategySelected == null) return;
                var strategicSkills = StrategySelected.StrategicSkills;

                await IsBusy.Loading();

                var strategicUserSkills = GetStrategicUserSkills(DateChoosed!.Value);
                var skills = ConvertToAIStrategicSkill.Convert(strategicSkills);
                var analysisResult = await StrategyVisionManager.GetStrategicSkillAnalysis(skills, strategicUserSkills, TeamMembers.Count);
                var skillResults = analysisResult.SkillResults;

                SkillResults = skillResults;
                GlobalResult = StrategyVisionManager.GetStrategicSkillResult(analysisResult, null, null, TeamMembers.Count);

                await IsBusy.Loading();
                SingleAnalysisMade = true;
            }
            catch (Exception err)
            {
                Snacks.Error(err.Message);
            }
            finally
            {
                await IsBusy.Loading();
            }
        }

        private async Task AutomateAnalysis()
        {
            await IsBusy.Loading();
            await DashboardAutomateAnalysis();
            await IsBusy.Loading();
        }

        public async Task DashboardAutomateAnalysis()
        {
            try
            {
                if (StrategySelected == null) return;
                var strategicSkills = StrategySelected.StrategicSkills;

                GlobalResult = null;

                DatesInRange.Clear();
                if (DatesRange.Start.HasValue && DatesRange.End.HasValue)
                {
                    for (var date = DatesRange.Start.Value; date <= DatesRange.End.Value; date = date.AddDays(1))
                    {
                        DatesInRange.Add(date);
                    }
                }

                foreach (var date in DatesInRange)
                {
                    try
                    {
                        var strategicUserSkills = GetStrategicUserSkills(date);
                        var skills = ConvertToAIStrategicSkill.Convert(strategicSkills);
                        var analysisResult = await StrategyVisionManager.GetStrategicSkillAnalysis(skills, strategicUserSkills, TeamMembers.Count);

                        var globalResult = StrategyVisionManager.GetStrategicSkillResult(analysisResult, null, null, TeamMembers.Count);
                        if (globalResult != null)
                        {
                            if (GlobalResult == null)
                                GlobalResult = globalResult;
                            else if (globalResult.GlobalNote > GlobalResult.GlobalNote &&
                                globalResult.UsedRatio >= GlobalResult.UsedRatio &&
                                globalResult.Score >= GlobalResult.Score)
                            {
                                GlobalResult = globalResult;
                                OptimalDate = date;
                            }
                        }

                        SkillResults = analysisResult.SkillResults;
                    }
                    catch (Exception err)
                    {
                        Console.WriteLine(err.Message);
                    }
                }

                SingleAnalysisMade = false;
            }
            catch (Exception err)
            {
                Snacks.Error(err.Message);
            }
        }

        private List<AIStrategicEmployeeSkill> GetStrategicUserSkills(DateTime targetDate)
        {
            List<AIStrategicEmployeeSkill> strategicUserSkills = [];

            List<UserSkill> userSkills = [];
            foreach (var member in TeamMembers)
            {
                foreach (var skill in member.UserSkills)
                {
                    skill.LastUse = skill.LastUse < targetDate ? targetDate : skill.LastUse;
                    if (userSkills.Any(x => x.MemberName == skill.MemberName && x.Name == skill.Name)) continue;
                    userSkills.Add(skill);
                }
            }

            foreach (var skill in userSkills)
            {
                var strategicSkill = strategicUserSkills.Where(x => x.Id == skill.Id)
                    .Where(x => x.EmployeeName == skill.MemberName)
                    .Where(x => x.SkillName == skill.Name)
                    .FirstOrDefault();

                double RR = skill.RetentionRate;
                double estimateRR = RetentionCalculator.RetentionDecrease(skill.Complexity, targetDate, skill.LastUse, RR);
                double averageRR = Math.Round((RR + estimateRR) / 2, 2);
                strategicUserSkills.Add(new AIStrategicEmployeeSkill()
                {
                    Id = skill.Id,
                    EmployeeName = skill.MemberName,
                    SkillName = skill.Name,
                    SkillComplexity = EnumsHelper.GetComplexity(skill.Complexity),
                    RetentionRate = RR,
                    EstimatedRR = estimateRR,
                    AverageRR = averageRR,
                });
            }
            return strategicUserSkills;
        }

        private async Task GenerateStrategicActionPlan()
        {
            try
            {
                await IsBusy.Loading();
                if (ActionPlanResult != null)
                {
                    ResetPlanGenerated();
                }

                DateTime optimalDate = SingleAnalysisMade ? DateChoosed!.Value : OptimalDate;
                await GenerateDashboard(optimalDate);
                await GetStrategicActionPlan(optimalDate);
                await InvokeAsync(StateHasChanged);
                await IsBusy.Loading();
            }
            catch (Exception err)
            {
                Snacks.Error(err.Message);
            }
        }

        private async Task GenerateDashboard(DateTime optimalDate)
        {
            await DashboardAutomateAnalysis();
            ActionPlanSkills = await StrategyVisionManager.CalculatePlan(SkillResults, optimalDate, SupportedLang.Eng);
        }

        private async Task GetStrategicActionPlan(DateTime optimalDate)
        {
            try
            {
                string? optimalDateResult;
                if (optimalDate == DateTime.MinValue) optimalDateResult = null;
                else optimalDateResult = $"**Optimal launching date** : {optimalDate.ToShortDateString()}\n";

                string? dashboardResult = null;
                if (GlobalResult!.Score > 0)
                    dashboardResult =
                        $"\n\n#### Metrics\n" +
                        $"**Ratio** : {GlobalResult!.UsedRatio:P2}\n" +
                        $"**Productivity** : {GlobalResult!.Score:P2}\n" +
                        $"**Success ratio** : {GlobalResult.GlobalNote:P2}\n" +
                        $"{optimalDateResult}";

                string type = StrategySelected!.Type;

                var datas = StrategyVisionManager.GetActionItems(ActionPlanSkills);
                AIStrategicActionRequest request = new()
                {
                    Lang = SupportedLang.Eng,
                    StartDate = OptimalDate,
                    TypeOfVision = EnumsHelper.GetTypeOfVision(type),
                    TextInput = StrategySelected!.Strategy,
                    Datas = datas,
                };

                ActionPlanResult = new();

                var response = await AIManager.GetStrategicActionPlan(request);
                Candidates = await GetCandidatSkillsMatcheds(response!.Candidates);
                response.ActionPlan = response.ActionPlan + dashboardResult;
                ActionPlanResult = new AIActionPlan()
                {
                    ActionPlan = response.ActionPlan,
                    Candidates = response.Candidates.Select(x => new AICandidateProfile() { Name = x.Name, Skills = x.Skills }).ToList(),
                    TrainingCostMin = response.TrainingCostMin,
                    RecruitmentCostMin = response.RecruitmentCostMin,
                    MixedCostMin = response.MixedCostMin,
                    TrainingCostMax = response.TrainingCostMax,
                    RecruitmentCostMax = response.RecruitmentCostMax,
                    MixedCostMax = response.MixedCostMax,
                    RecommandationCost = response.RecommandationCost,
                };
            }
            catch (Exception err)
            {
                Snacks.Error(err.Message);
            }
        }

        private async Task<List<StrategyCandidates>> GetCandidatSkillsMatcheds(List<AICandidateProfile> profiles)
        {
            List<StrategyCandidates> candidates = [];
            try
            {
                var employes = ConvertToEmployee.Convert(TeamMembers);
                foreach (var profile in profiles)
                {
                    List<UserSkill> requiredSkills = [];
                    foreach (var skillName in profile.Skills)
                    {
                        var skillRef = Skills.FirstOrDefault(x => x.Name == skillName);
                        if (skillRef == null) continue;
                        requiredSkills.Add(new UserSkill()
                        {
                            Name = skillName,
                            Complexity = skillRef.Complexity,
                        });
                    }

                    if (requiredSkills.Count == 0) continue;
                    var employeeSkills = ConvertToEmployeeSkill.Convert(requiredSkills);
                    var results = await EmployeeManager.GetTopCandidate(employeeSkills, employes, 3);
                    candidates.Add(new StrategyCandidates()
                    {
                        JobTitle = profile.Name,
                        Results = results,
                    });
                }
            }
            catch (Exception err)
            {
                Snacks.Error(err.Message);
            }
            return candidates;
        }
        #endregion
    }
}
