using TalentFlow.Demo.DataGen;
using TalentFlow.Demo.Models;
using MudBlazor;
using TalentFlow.Csharp.Core.Services;
using TalentFlow.Csharp.Models.Enums;
using TalentFlow.Csharp.Models.Objects;

namespace TalentFlow.Demo.Components.Pages
{
    public partial class PerfectCandidates
    {
        /// <summary>
        /// Allows you to retrieve the activation status of license in TalentFlow.Csharp.Core 
        /// </summary>
        #region TalentFlow.Csharp.AI status configuration
        private bool IsActivated { get; set; }
        public void LicenseActivationStatus(bool config) => IsActivated = config;
        #endregion

        // Properties for data management
        private List<TeamMember> TeamMembers = [];
        private List<UserSkill> AvailableSkills = [];
        private List<UserSkill> FilteredAvailableSkills = [];
        private List<UserSkill> SelectedSkills = [];
        private List<CandidatSkillsMatched> EvaluationResults = [];

        // Properties for the user interface
        private string searchText = string.Empty;
        private bool isRandomizing = false;
        private bool isClearing = false;
        private bool isEvaluating = false;
        private bool hasEvaluated = false;

        protected override async Task OnInitializedAsync()
        {
            // Initialize the sample data
            await GenerateData();
            FilteredAvailableSkills = AvailableSkills.ToList();
        }

        private void FilterSkills()
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                FilteredAvailableSkills = AvailableSkills.Where(s => !SelectedSkills.Contains(s)).ToList();
            }
            else
            {
                FilteredAvailableSkills = AvailableSkills
                    .Where(s => !SelectedSkills.Contains(s) &&
                               (s.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                                s.Complexity.Contains(searchText, StringComparison.OrdinalIgnoreCase)))
                    .ToList();
            }
        }

        private void SelectSkill(UserSkill skill)
        {
            if (!SelectedSkills.Contains(skill))
            {
                SelectedSkills.Add(skill);
                FilterSkills(); // Update the filtered list
            }
        }

        private void RemoveSkill(UserSkill skill)
        {
            SelectedSkills.Remove(skill);
            FilterSkills(); // Update the filtered list
        }

        private async Task ClearSelection()
        {
            isClearing = true;
            await InvokeAsync(StateHasChanged);

            SelectedSkills.Clear();
            FilterSkills();
            EvaluationResults.Clear();
            hasEvaluated = false;
            Snacks.Info("Selection cleared");
            await Task.Delay(1000);

            isClearing = false;
            await InvokeAsync(StateHasChanged);
        }

        private async Task EvaluateCandidates()
        {
            if (!SelectedSkills.Any())
            {
                Snacks.Warning("Please select at least one skill");
                return;
            }

            isEvaluating = true;
            hasEvaluated = false;

            try
            {
                EvaluationResults = await CalculateRetentionScores(TeamMembers, SelectedSkills);
                hasEvaluated = true;

                Snacks.Success($"Evaluation Completed - {EvaluationResults.Count} candidates analyzed");
            }
            catch (Exception ex)
            {
                Snacks.Error($"Error while evaluating: {ex.Message}");
            }
            finally
            {
                isEvaluating = false;
            }
        }

        private async Task RefreshResults()
        {
            if (SelectedSkills.Any())
            {
                await EvaluateCandidates();
            }
        }

        // Utility methods for colors
        private Color GetColorTopSelection(int top)
        {
            return top switch
            {
                >= 8 => Color.Error,
                >= 6 => Color.Warning,
                _ => Color.Success,
            };
        }

        // PLACEHOLDER: Method to implement to calculate retention scores
        private int Top = 3;
        private async Task<List<CandidatSkillsMatched>> CalculateRetentionScores(List<TeamMember> teamMembers, List<UserSkill> requiredSkills)
        {
            var employees = ConvertToEmployee.Convert(teamMembers);
            var employeeSkills = ConvertToEmployeeSkill.Convert(requiredSkills);
            await Task.Delay(1000);
            var results = await EmployeeManager.GetTopCandidate(employeeSkills, employees, Top);
            return results;
        }

        // Methods for generating sample data - to be removed in production
        private async Task RandomizeData()
        {
            AvailableSkills.Clear();
            TeamMembers.Clear();
            isRandomizing = true;
            await InvokeAsync(StateHasChanged);

            await GenerateData();
            await Task.Delay(1000);

            isRandomizing = false;
            await InvokeAsync(StateHasChanged);
        }

        private Task GenerateData()
        {
            Generator.GenerateEmployeeSkills();
            TeamMembers = Generator.TeamMembers;
            AvailableSkills = Generator.UserSkills;
            return Task.CompletedTask;
        }

        private static List<string> Complexities { get; } =
        [
            nameof(Complexity.Basic),
            nameof(Complexity.Moderate),
            nameof(Complexity.Complex)
        ];
    }
}
