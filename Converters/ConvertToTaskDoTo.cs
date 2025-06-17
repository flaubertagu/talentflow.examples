using TalentFlow.Demo.Models;
using TalentFlow.Csharp.Models.Objects;

namespace TalentFlow.Demo.Converters
{
    public class ConvertToTaskDoTo : IConverter<TaskAttribution, TaskDoTo>
    {
        public TaskDoTo Convert(TaskAttribution source)
        {
            return new TaskDoTo()
            {
                Id = source.Id,
                TaskName = source.Title,
                TaskDescription = source.Description,
                AssignedTo = source.EmployeeName,
                StartDate = source.StartDate,
                EndDate = source.EndDate,
                UserScore = source.EmployeScore,
                RequiredScore = source.MaxScore,
                OverBooked = source.OverBooked,
                OverBookRatio = source.OverBookRatio,
                Category = (int)source.Category,
                AverageRRincrease = source.AverageRRincrease,
                UserSkills = source.RequiredSkills.Select(skill => new UserSkill()
                {
                    Id = skill.Id,
                    MemberName = skill.EmployeeName,
                    Name = skill.SkillName,
                    Complexity = nameof(skill.SkillComplexity),
                    RetentionRate = skill.RetentionRate,
                    LastUse = skill.LastUse,
                    RelearningDuration = skill.RelearningDuration,
                }).ToList(),
                RelearningDuration = source.RelearningDuration,
                RelearningStartDate = source.RelearningStartDate,
                RelearningEndDate = source.RelearningEndDate,
            };
        }

        public TaskDoTo? ConvertNull(TaskAttribution? source)
        {
            if (source == null) return null;
            return Convert(source);
        }

        public List<TaskDoTo> Convert(List<TaskAttribution> sourceList)
        {
            return sourceList?.Select(Convert).ToList() ?? [];
        }
    }
}
