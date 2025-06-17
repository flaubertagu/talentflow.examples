using TalentFlow.Demo.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using TalentFlow.Csharp.Models.Enums;

namespace TalentFlow.Demo.Components.Shared
{
    public partial class SkillListVisualizer
    {
        [Parameter]
        public string? Title { get; set; } = string.Empty;
        [Parameter]
        public string? EmployeeName { get; set; } = string.Empty;
        [Parameter]
        public List<UserSkill>? UserSkills { get; set; } = [];
        [Parameter]
        public EventCallback OnClose { get; set; }

        private Color GetComplexityChipColor(string complexity)
        {
            return complexity?.ToLower() switch
            {
                nameof(Complexity.Basic) => Color.Success,
                nameof(Complexity.Moderate) => Color.Warning,
                nameof(Complexity.Complex) => Color.Error,
                _ => Color.Default
            };
        }

        private Color GetRetentionProgressColor(double retentionRate)
        {
            return retentionRate switch
            {
                >= 0.8 => Color.Success,
                >= 0.6 => Color.Warning,
                >= 0.4 => Color.Info,
                _ => Color.Error
            };
        }
    }
}
