using TalentFlow.Demo.Models;
using TalentFlow.Csharp.Core.Helpers;
using TalentFlow.Csharp.Models.Objects;

namespace TalentFlow.Demo.Converters
{
    public class ConvertToTaskFinished : IConverter<TaskCompleted, TaskFinished>
    {
        public TaskFinished Convert(TaskCompleted source)
        {
            return new TaskFinished()
            {
                Title = source.TaskName,
                Description = source.TaskDescription,
                EmployeeName = source.AssignedTo,
                StartDate = source.StartDate,
                EndDate = source.EndDate,
                DeliveryDate = source.DeliveryDate == DateTime.MinValue ? source.EndDate : source.DeliveryDate,
                Occurence = source.Occurence, //Default = 1;
                RequiredSkills = source.TaskSkills.Select(x => new TaskSkill()
                {
                    SkillName = x.Name,
                    SkillComplexity = EnumsHelper.GetComplexity(x.Complexity),
                }).ToList(),
            };
        }

        public TaskFinished? ConvertNull(TaskCompleted? source)
        {
            if (source == null) return null;
            return Convert(source);
        }

        public List<TaskFinished> Convert(List<TaskCompleted> sourceList)
        {
            return sourceList?.Select(Convert).ToList() ?? [];
        }
    }
}
