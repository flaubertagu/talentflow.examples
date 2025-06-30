using MudBlazor;
using TalentFlow.Csharp.Models.Enums;
using TalentFlow.Csharp.Models.Objects;
using TalentFlow.Demo.Components.Shared;
using TalentFlow.Demo.DataGen;
using TalentFlow.Demo.Models;

namespace TalentFlow.Demo.Components.Pages
{
    public partial class SkillsSaver
    {
        public IsBusy IsBusy { get; set; } = new();

        /// <summary>
        /// Allows you to retrieve the activation status of license in TalentFlow.Csharp.Core 
        /// </summary>
        #region TalentFlow.Csharp.AI status configuration
        private bool IsActivated { get; set; }
        public void LicenseActivationStatus(bool config) => IsActivated = config;
        #endregion

        protected override async Task OnInitializedAsync()
        {
            await GenerateData();
        }

        private async Task RegenerateData()
        {
            TasksFinished.Clear();
            EmployeesBeforeData.Clear();
            EmployeesAfterData.Clear();
            await GenerateData();
            StateHasChanged();
        }

        private Task GenerateData()
        {
            Generator.GenerateTaskCompleted();
            var tasksCompleted = Generator.TasksCompleted;
            TasksFinished = ConvertToTaskFinished.Convert(tasksCompleted);

            var teamMembers = Generator.TeamMembers;
            var members = teamMembers.Select(x => new TeamMember()
            {
                Id = x.Id,
                Name = x.Name,
                Email = x.Email,
                UserSkills = x.UserSkills.Select(y => new UserSkill()
                {
                    Id = y.Id,
                    MemberName = y.MemberName,
                    Name = y.Name,
                    Complexity = y.Complexity,
                    RetentionRate = 0,
                    LastUse = DateTime.MinValue,
                    RelearningDuration = 0,
                }).ToList(),
            }).ToList();
            EmployeesBeforeData = ConvertToEmployee.Convert(members);
            SetFilterLists();
            return Task.CompletedTask;
        }

        // Data properties for binding
        private List<TaskFinished> TasksFinished { get; set; } = [];
        private List<Employee> EmployeesBeforeData { get; set; } = [];
        private List<Employee> EmployeesAfterData { get; set; } = [];

        #region Filterd functions
        private string searchText = string.Empty;
        private List<Employee> FilteredEmployeesBeforeData { get; set; } = [];
        private List<Employee> FilteredEmployeesAfterData { get; set; } = [];

        private void SetFilterLists()
        {
            FilteredEmployeesBeforeData = EmployeesBeforeData.ToList();
            FilteredEmployeesAfterData = EmployeesAfterData.ToList();
        }

        private void FilterSkills()
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                SetFilterLists();
            }
            else
            {
                FilteredEmployeesBeforeData = EmployeesBeforeData
                    .Where(s => s.EmployeeName.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                FilteredEmployeesAfterData = EmployeesAfterData
                    .Where(s => s.EmployeeName.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
        }
        #endregion

        private async Task CalculateMetrics()
        {
            try
            {
                await IsBusy.Loading();
                EmployeesAfterData.Clear();
                var employeeBeforeData = GetEmployeeBeforeData(EmployeesBeforeData);
                EmployeesAfterData = await EmployeeManager.CalculateSkillRetention(TasksFinished, employeeBeforeData);
                SetFilterLists();
            }
            catch (Exception err)
            {
                Snacks.Error(err.Message);
            }
            finally
            {
                await IsBusy.Loading();
                StateHasChanged();
            }
        }

        private List<Employee> GetEmployeeBeforeData(List<Employee> employeeBeforeData)
        {
            if (employeeBeforeData.Count == 0) return [];
            List<Employee> employees = [];
            employees = employeeBeforeData.Select(x => new Employee()
            {
                EmployeeName = x.EmployeeName,
                EmployeSkills = x.EmployeSkills.Select(y => new EmployeeSkill()
                {
                    EmployeeName = y.EmployeeName,
                    SkillName = y.SkillName,
                    SkillComplexity = y.SkillComplexity,
                    RetentionRate = 0,
                    LastUse = DateTime.MinValue,
                    RelearningDuration = 0,
                }).ToList(),
            }).ToList();
            return employees;
        }

        private bool TasksExpanded = false;
        private void OnTaskExpandedClick()
        {
            TasksExpanded = !TasksExpanded;
        }

        // Helper method for complexity color coding
        private Color GetComplexityColor(Complexity complexity)
        {
            return complexity switch
            {
                Complexity.Basic => Color.Success,
                Complexity.Moderate => Color.Warning,
                Complexity.Complex => Color.Error,
                _ => Color.Default
            };
        }
    }
}
