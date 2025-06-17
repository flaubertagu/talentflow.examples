using TalentFlow.Demo.Components.Shared;
using TalentFlow.Demo.DataGen;
using TalentFlow.Demo.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using TalentFlow.Csharp.Models.Objects;

namespace TalentFlow.Demo.Components.Pages
{
    public partial class EmployeeMetrics
    {
        /// <summary>
        /// Allows you to retrieve the activation status of license in TalentFlow.Csharp.Core 
        /// </summary>
        #region TalentFlow.Csharp.AI status configuration
        private bool IsActivated { get; set; }
        public void LicenseActivationStatus(bool config) => IsActivated = config;

        #endregion
        [Parameter]
        public List<TeamMember> TeamMembers { get; set; } = [];
        [Parameter]
        public List<TaskCompleted> TasksCompleted { get; set; } = [];
        public IsBusy IsBusy { get; set; } = new();
        private DialogOptions _dialogOptions { get; } = new()
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseButton = false,
            CloseOnEscapeKey = false,
            BackgroundClass = "my-custom-class"
        };
        private bool _popupVisible => _skillsDialogVisible || _metricsDialogVisible;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await GenerateData();
            }
        }

        private async Task GenerateData()
        {
            Generator.GenerateTaskCompleted();
            TeamMembers = Generator.TeamMembers;
            TasksCompleted = Generator.TasksCompleted;
            await GetAllEmployeeMetrics();
            //Not recommended for test this feature will consume faster availabled remaining usage
            //await GetEmployeesMetrics();
        }

        #region Search TeamMember
        // Component status for search and pagination
        private string _searchValue = string.Empty;
        private int _currentPage = 0;
        private TeamMember? _selectedMember;

        // Pagination configuration
        private const int ItemsPerPage = 9; // 3 colonnes × 3 lignes = 9 éléments max par page

        // Calculated properties
        private IEnumerable<TeamMember> FilteredMembers =>
            string.IsNullOrEmpty(_searchValue)
                ? TeamMembers
                : TeamMembers.Where(m => m.Name.Contains(_searchValue, StringComparison.OrdinalIgnoreCase));

        private IEnumerable<TeamMember> CurrentPageMembers =>
            FilteredMembers.Skip(_currentPage * ItemsPerPage).Take(ItemsPerPage);

        private int TotalPages => (int)Math.Ceiling((double)FilteredMembers.Count() / ItemsPerPage);

        // Research methods
        private async Task<IEnumerable<string>> SearchMembers(string value, CancellationToken cancellationToken)
        {
            // Simulate a small delay for user experience (optional)
            await Task.Delay(100, cancellationToken);

            // If the value is empty, return all names
            if (string.IsNullOrEmpty(value))
                return TeamMembers.Select(m => m.Name);

            // Filter names that contain the searched value
            return TeamMembers
                .Where(m => m.Name.Contains(value, StringComparison.OrdinalIgnoreCase))
                .Select(m => m.Name);
        }

        private void ClearSearch()
        {
            _searchValue = string.Empty;
            _currentPage = 0; // Return to first page after search
            StateHasChanged();
        }

        // Navigation methods
        private void NextPage()
        {
            if (_currentPage < TotalPages - 1)
            {
                _currentPage++;
                StateHasChanged();
            }
        }

        private void PreviousPage()
        {
            if (_currentPage > 0)
            {
                _currentPage--;
                StateHasChanged();
            }
        }

        // Watch to reset pagination when changing search
        protected override void OnParametersSet()
        {
            // Reset current page if search changes
            _currentPage = 0;
        }
        #endregion

        #region Skills
        // Options for dialog
        private bool _skillsDialogVisible = false;

        private async Task ShowSkillsDialog(TeamMember member)
        {
            await IsBusy.Loading();
            try
            {
                _selectedMember = member;
                _skillsDialogVisible = true;
                StateHasChanged();
            }
            catch (Exception err)
            {
                Snacks.Error(err.Message);
            }
            await IsBusy.Loading();
        }

        private void CloseSkillsDialog()
        {
            _skillsDialogVisible = false;
            _selectedMember = null;
            StateHasChanged();
        }
        #endregion

        #region Metrics
        private bool _metricsDialogVisible = false;
        private EmployeMetrics? _selectedEmployeeMetrics;
        private List<EmployeMetrics> Metrics { get; set; } = [];
        private bool _isProcessing;
        private double Progression;
        private async Task GetAllEmployeeMetrics()
        {
            Metrics.Clear();
            _isProcessing = true;
            Progression = 0;
            try
            {
                double count = 0;
                List<TaskFinished> allTasks = [];
                List<Employee> employees = [];
                foreach (var member in TeamMembers)
                {
                    var taskCompleted = TasksCompleted.Where(x => x.AssignedTo == member.Name).ToList();
                    var tasksFinished = ConvertToTaskFinished.Convert(taskCompleted);
                    var employee = ConvertToEmployee.Convert(member);
                    allTasks.AddRange(tasksFinished);
                    employees.Add(employee);

                    count++;
                    Progression = (count / TeamMembers.Count * 100) - 1;
                    await InvokeAsync(StateHasChanged);
                    await Task.Delay(50);
                }

                Metrics = await EmployeeManager.GetEmployeesMetrics(allTasks, employees);
                Progression++;
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception err)
            {
                Snacks.Error(err.Message);
            }
            _isProcessing = false;
            Progression = 100;
            await InvokeAsync(StateHasChanged);
        }

        //Not recommended for test this feature will consume faster availabled remaining usage
        private async Task GetEmployeesMetrics()
        {
            Metrics.Clear();
            _isProcessing = true;
            Progression = 0;
            double count = 0;
            foreach (var member in TeamMembers)
            {
                var taskCompleted = TasksCompleted.Where(x => x.AssignedTo == member.Name).ToList();
                var tasksFinished = ConvertToTaskFinished.Convert(taskCompleted);
                var employee = ConvertToEmployee.Convert(member);
                var metrics = await EmployeeManager.GetEmployeeMetrics(tasksFinished, employee);
                Metrics.Add(metrics);

                count++;
                Progression = count / TeamMembers.Count * 100;
                await InvokeAsync(StateHasChanged);
                await Task.Delay(100);
            }
            _isProcessing = false;
            Progression = 100;
            await InvokeAsync(StateHasChanged);
        }

        private async Task GetMetrics(TeamMember member)
        {
            await IsBusy.Loading();
            try
            {
                _selectedEmployeeMetrics = Metrics.FirstOrDefault(x => x.EmployeeName == member.Name);
                _metricsDialogVisible = true;
                StateHasChanged();
            }
            catch (Exception err)
            {
                Snacks.Error(err.Message);
            }
            await IsBusy.Loading();
        }

        private void CloseMetricsDialog()
        {
            _metricsDialogVisible = false;
            _selectedEmployeeMetrics = null;
            _selectedMember = null;
            StateHasChanged();
        }
        // Utility methods for metrics
        private Color GetDelayRatioColor(double ratio)
        {
            return ratio switch
            {
                >= 0.8 => Color.Success,
                >= 0.6 => Color.Warning,
                >= 0.4 => Color.Info,
                _ => Color.Error
            };
        }

        private string GetDelayRatioLabel(double ratio)
        {
            return ratio switch
            {
                >= 0.9 => "Worrying delay rate",
                >= 0.8 => "Very high delay rate",
                >= 0.7 => "High delay rate",
                >= 0.6 => "Significant delay rate",
                <= 0.2 => "Acceptable delay rate",
                _ => "Delay rate to improve"
            };
        }

        private Color GetProductivityColor(double ratio)
        {
            return ratio switch
            {
                >= 0.8 => Color.Success,
                >= 0.6 => Color.Info,
                >= 0.4 => Color.Warning,
                _ => Color.Error
            };
        }

        private string GetProductivityLabel(double ratio)
        {
            return ratio switch
            {
                >= 0.9 => "Excellent productivity",
                >= 0.8 => "Very good productivity",
                >= 0.7 => "Good productivity",
                >= 0.6 => "Adequate productivity",
                >= 0.5 => "Productivity needs improvement",
                _ => "Low productivity"
            };
        }

        private Color GetRetentionScoreColor(double score)
        {
            return score switch
            {
                >= 0.85 => Color.Success,
                >= 0.70 => Color.Warning,
                >= 0.55 => Color.Info,
                _ => Color.Error
            };
        }

        private string GetRetentionScoreLabel(double score)
        {
            return score switch
            {
                >= 0.9 => "Excellent retention",
                >= 0.8 => "Very good retention",
                >= 0.7 => "Good retention",
                >= 0.6 => "Adequate retention",
                _ => "Retention needs improvement"
            };
        }

        private Color GetMalusScoreColor(double score)
        {
            return score switch
            {
                >= 0.75 => Color.Error,
                >= 0.40 => Color.Warning,
                >= 0.25 => Color.Info,
                _ => Color.Info
            };
        }

        private string GetMalusScoreLabel(double score)
        {
            return score switch
            {
                >= 0.9 => "Worrying penalty",
                >= 0.8 => "Very high penalty",
                >= 0.7 => "High penalty",
                >= 0.6 => "Significant penalty",
                <= 0.2 => "Acceptable penalty",
                _ => "Penalty to improve"
            };
        }

        private Color GetPerformanceColor(double? score)
        {
            return score switch
            {
                >= 0.85 => Color.Success,
                >= 0.60 => Color.Info,
                >= 0.50 => Color.Warning,
                _ => Color.Error
            };
        }

        private string GetPerformanceLabel(double score)
        {
            return score switch
            {
                >= 0.90 => "Exceptional performance",
                >= 0.80 => "Very good performance",
                >= 0.70 => "Good performance",
                >= 0.60 => "Satisfactory performance",
                >= 0.50 => "Performance needs improvement",
                _ => "Insufficient performance"
            };
        }

        private string GetUserScoreLabel(double score)
        {
            return score switch
            {
                >= 0.90 => "Exceptional score",
                >= 0.80 => "Very good score",
                >= 0.70 => "Good score",
                >= 0.60 => "Satisfactory score",
                >= 0.50 => "Score needs improvement",
                _ => "Insufficient score"
            };
        }

        private Severity GetOverallSeverity(EmployeMetrics metrics)
        {
            return metrics.PerformanceScore switch
            {
                >= 0.8 => Severity.Success,
                >= 0.6 => Severity.Warning,
                >= 0.4 => Severity.Info,
                _ => Severity.Error
            };
        }

        private string GetOverallAssessment(EmployeMetrics metrics)
        {
            return metrics.PerformanceScore switch
            {
                >= 0.85 => "Excellent",
                >= 0.75 => "Very good",
                >= 0.65 => "Good",
                >= 0.55 => "Satisfying",
                >= 0.45 => "To Improve",
                _ => "Critic"
            };
        }

        private string GetOverallRecommendation(EmployeMetrics metrics)
        {
            return metrics.PerformanceScore switch
            {
                >= 0.85 => "Model employee. Consider for leadership or mentoring roles.",
                >= 0.75 => "Very good performance. Consider development opportunities.",
                >= 0.65 => "Solid performance. Continue skill development.",
                >= 0.55 => "Adequate performance. Targeted training recommended.",
                >= 0.45 => "Improvement needed. Development plan required.",
                _ => "Immediate action required. Enhanced support needed."
            };
        }

        private double scoreOn5Stars(double value, double maxScore)
        {
            return value / maxScore * 5;
        }
        #endregion
    }
}
