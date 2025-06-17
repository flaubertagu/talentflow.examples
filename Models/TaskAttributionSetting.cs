namespace TalentFlow.Demo.Models
{
    public class TaskAttributionSetting
    {
        public List<TaskCompleted> TasksCompleted { get; set; } = [];
        public List<TaskDoTo> TasksToExecute { get; set; } = [];
        public List<EmployeeUnavailability> Unavailabilities { get; set; } = [];
        public List<TeamMember> TeamMembers { get; set; } = [];
        public TaskDoTo? TaskEdited { get; set; } //Value can be null
        public required TaskSetting TaskSetting { get; set; }
    }
}
