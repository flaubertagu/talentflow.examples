using TalentFlow.Demo.Models;
using TalentFlow.Csharp.Models.Objects;

namespace TalentFlow.Demo.Converters
{
    public class ConvertToTaskAttributionInput(
        IConverter<TaskCompleted, TaskFinished> taskCompleted,
        IConverter<TaskDoTo, TaskAttribution> taskAugmented,
        IConverter<TeamMember, Employee> teamMember,
        IConverter<EmployeeUnavailability, EmployeeUnavailabilities> unavailabilities,
        IConverter<TaskSetting, AttributionSetting> taskSetting) : IConverter<TaskAttributionSetting, TaskAttributionInput>
    {
        private readonly IConverter<TaskCompleted, TaskFinished> taskCompleted = taskCompleted;
        private readonly IConverter<TaskDoTo, TaskAttribution> taskAugmented = taskAugmented;
        private readonly IConverter<TeamMember, Employee> teamMember = teamMember;
        private readonly IConverter<EmployeeUnavailability, EmployeeUnavailabilities> unavailabilities = unavailabilities;
        private readonly IConverter<TaskSetting, AttributionSetting> taskSetting = taskSetting;

        public TaskAttributionInput Convert(TaskAttributionSetting source)
        {
            var taskFinished = taskCompleted.Convert(source.TasksCompleted);
            var tasksToDo = taskAugmented.Convert(source.TasksToExecute);
            var employees = teamMember.Convert(source.TeamMembers);
            var employeUnavailabilities = unavailabilities.Convert(source.Unavailabilities);
            var taskEdited = taskAugmented.ConvertNull(source.TaskEdited); //Value can be null
            var setting = taskSetting.Convert(source.TaskSetting);
            return new TaskAttributionInput()
            {
                TaskFinished = taskFinished,
                TasksToDo = tasksToDo,
                Employees = employees,
                Unavailabilities = employeUnavailabilities,
                TaskEdited = taskEdited,
                AttributionSetting = setting,
            };
        }

        public TaskAttributionInput? ConvertNull(TaskAttributionSetting? source)
        {
            if (source == null) return null;
            return Convert(source);
        }

        public List<TaskAttributionInput> Convert(List<TaskAttributionSetting> sourceList)
        {
            return sourceList?.Select(Convert).ToList() ?? [];
        }
    }
}
