using TalentFlow.Demo.Models;
using TalentFlow.Csharp.Core.Helpers;
using TalentFlow.Csharp.Models.Objects;

namespace TalentFlow.Demo.Converters
{
    public class ConvertToTaskAttribution : IConverter<TaskDoTo, TaskAttribution>
    {
        public TaskAttribution Convert(TaskDoTo source)
        {
            return new TaskAttribution()
            {
                Id = source.Id,
                Title = source.TaskName,
                Description = source.TaskDescription,
                EmployeeName = source.AssignedTo,
                StartDate = source.StartDate,
                EndDate = source.EndDate,
                Category = EnumsHelper.CandidateCategory(source.Category),
                RequiredSkills = source.UserSkills.Select(skill => new EmployeeSkill()
                {
                    EmployeeName = skill.MemberName,
                    SkillName = skill.Name,
                    SkillComplexity = EnumsHelper.GetComplexity(skill.Complexity),
                    RetentionRate = skill.RetentionRate,
                    LastUse = skill.LastUse,
                }).ToList(),
            };
        }

        public TaskAttribution? ConvertNull(TaskDoTo? source)
        {
            if (source == null) return null;
            return Convert(source);
        }

        public List<TaskAttribution> Convert(List<TaskDoTo> sourceList)
        {
            return sourceList?.Select(Convert).ToList() ?? [];
        }
    }
}
