using TalentFlow.Demo.Components.Shared;
using TalentFlow.Demo.DataGen;
using TalentFlow.Demo.Models;
using MudBlazor;
using TalentFlow.Csharp.Core.Helpers;
using TalentFlow.Csharp.Models.Enums;

namespace TalentFlow.Demo.Components.Pages
{
    public partial class Task_attribution
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
            TeamMembers.Clear();
            TasksCompleted.Clear();
            TasksToDo.Clear();
            Unavailabilities.Clear();
            TasksOptimized.Clear();
            await GenerateData();
            if (_ShowTaskSetting) _ShowTaskSetting = false;
            _ShowTaskToDo = true;
            StateHasChanged();
        }

        private Task GenerateData()
        {
            Generator.GenerateTaskToDo();
            TeamMembers = Generator.TeamMembers;
            TasksCompleted = Generator.TasksCompleted;
            TasksToDo = Generator.TasksToDo;
            Unavailabilities = Generator.Unavailabilities;
            return Task.CompletedTask;
        }

        private List<TeamMember> TeamMembers { get; set; } = [];
        private List<TaskCompleted> TasksCompleted { get; set; } = [];
        private List<TaskDoTo> TasksToDo { get; set; } = [];
        private List<EmployeeUnavailability> Unavailabilities { get; set; } = [];
        private List<TaskDoTo> TasksOptimized { get; set; } = [];
        private TaskSetting TaskSetting { get; set; } = new();

        bool _ShowTaskToDo = true;
        private void OnShowTaskToDoClick()
        {
            _ShowTaskToDo = !_ShowTaskToDo;
            if (_ShowTaskSetting) _ShowTaskSetting = false;
        }

        bool _ShowTaskSetting = false;
        private void OnShowTaskSettingClick()
        {
            _ShowTaskSetting = !_ShowTaskSetting;
            if (_ShowTaskToDo) _ShowTaskToDo = false;
        }
        private Color RecruitementColor(bool value)
        {
            return value switch
            {
                true => Color.Success,
                false => Color.Error,
            };
        }

        private bool _isAutomting;
        public async Task LaunchTaskAttribution()
        {
            if (_ShowTaskToDo) _ShowTaskToDo = false;
            if (_ShowTaskSetting) _ShowTaskSetting = false;
            _isAutomting = true;
            TasksOptimized.Clear();
            StateHasChanged();

            await Automate();

            _isAutomting = false;
            StateHasChanged();
        }

        private async Task Automate()
        {
            try
            {
                var tasksToOptimize = TasksToDo.ToList();
                var taskCompleted = TasksCompleted.ToList();

                var tasksOptimized = await OptimizeTaskDoTo(new TaskAttributionSetting()
                {
                    TasksCompleted = TasksCompleted,
                    TasksToExecute = TasksToDo,
                    TeamMembers = TeamMembers,
                    Unavailabilities = Unavailabilities,
                    TaskEdited = null,
                    TaskSetting = TaskSetting,
                });
                if (tasksOptimized != null) TasksOptimized = tasksOptimized;
            }
            catch (Exception err)
            {
                Snacks.Simple(err.Message);
            }
        }

        private async Task<List<TaskDoTo>> OptimizeTaskDoTo(TaskAttributionSetting setting)
        {
            var inputs = ConvertToTaskAttributionInput.Convert(setting);
            var tasksOptimized = await TaskAttributionManager.TaskOptimizationResult(inputs);
            if (tasksOptimized == null) return [];
            return ConvertToTaskDoTo.Convert(tasksOptimized);
        }

        private TaskDoTo? TaskSelected;
        private void TaskOptimizedSelected(TableRowClickEventArgs<TaskDoTo> task) => TaskSelected = task.Item;

        private void CancelTaskOptimizedSelection() => TaskSelected = null;

        private async Task ValidateMemberSelected(TeamMember memberSelected)
        {
            if (TaskSelected == null) return;
            await IsBusy.Loading();
            try
            {
                var task = TaskSelected;
                task.AssignedTo = memberSelected.Name;

                var employee = ConvertToEmployee.Convert(memberSelected);
                var taskAttribution = ConvertToTaskAttribution.Convert(task);
                var inputs = ConvertToTaskAttributionInput.Convert(new TaskAttributionSetting()
                {
                    TasksCompleted = TasksCompleted,
                    TasksToExecute = TasksOptimized,
                    TeamMembers = TeamMembers,
                    Unavailabilities = Unavailabilities,
                    TaskEdited = null,
                    TaskSetting = TaskSetting,
                });
                var memberScoreResult = await TaskAttributionManager.TaskMemberScore(employee, taskAttribution, inputs);
                var taskEdited = ConvertToTaskDoTo.Convert(memberScoreResult);
                task.Category = taskEdited.Category;

                var tasksOptimized = await OptimizeTaskDoTo(new TaskAttributionSetting()
                {
                    TasksCompleted = TasksCompleted,
                    TasksToExecute = TasksToDo,
                    TeamMembers = TeamMembers,
                    Unavailabilities = Unavailabilities,
                    TaskEdited = taskEdited,
                    TaskSetting = TaskSetting,
                });
                TasksOptimized = tasksOptimized;
            }
            catch (Exception err)
            {
                Snacks.Error(err.Message);
            }
            await IsBusy.Loading();
            CancelTaskOptimizedSelection();
            StateHasChanged();
        }

        private bool ShowSkillsActivated = false;
        private void OnShowSkillSelection(bool value) => ShowSkillsActivated = value;

        private bool ShowDatesActivated = false;
        private void OnShowDateSelection(bool value) => ShowDatesActivated = value;

        public Color ScoringColor(int category)
        {
            switch (category)
            {
                case 1:
                    return Color.Error;
                case 2:
                    return Color.Warning;
                case 3:
                    return Color.Success;
                default:
                    return Color.Info;
            }
        }

        public static string CandidateCategory(int categoryInt)
        {
            var category = EnumsHelper.CandidateCategory(categoryInt);
            switch (category)
            {
                case AttributionCategory.BadCategory:
                    return "Bad";
                case AttributionCategory.GoodCategory:
                    return "Good";
                case AttributionCategory.PerfectCategory:
                    return "Perfect";
                default:
                    return "Recruitment";
            }
        }

        public Color ColorOverBook(double? ratio)
        {
            if (ratio == null) return Color.Default;
            if (ratio < 0.5)
                return Color.Default;
            else if (ratio <= 1)
                return Color.Warning;
            else
                return Color.Error;
        }
    }
}
