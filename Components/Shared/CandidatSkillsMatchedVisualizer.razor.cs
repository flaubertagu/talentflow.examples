using TalentFlow.Demo.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using TalentFlow.Csharp.Models.Objects;

namespace TalentFlow.Demo.Components.Shared
{
    public partial class CandidatSkillsMatchedVisualizer
    {
        [Parameter]
        public string Title { get; set; } = string.Empty;

        [Parameter]
        public List<CandidatSkillsMatched> Results { get; set; } = [];

        private bool _skillsDialogVisible = false;
        private TeamMember? SelectedMember;
        private DialogOptions _dialogOptions { get; } = new()
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseButton = false,
            CloseOnEscapeKey = false,
            BackgroundClass = "my-custom-class"
        };

        private void OnResultSelected(DataGridRowClickEventArgs<CandidatSkillsMatched> args)
        {
            if (args == null) return;
            SelectedMember = new()
            {
                Name = args.Item.EmployeeName,
                UserSkills = ConvertToUserSkill.Convert(args.Item.Skills),
            };
            _skillsDialogVisible = true;
        }

        private void CloseSkillsDialog()
        {
            _skillsDialogVisible = false;
            SelectedMember = null;
            StateHasChanged();
        }

        private Color GetMatchColor(double percentage)
        {
            return (percentage * 100) switch
            {
                >= 80 => Color.Success,
                >= 60 => Color.Warning,
                _ => Color.Error
            };
        }

        private Color GetScoreColor(double score)
        {
            return (score * 100) switch
            {
                >= 80 => Color.Success,
                >= 60 => Color.Info,
                >= 40 => Color.Warning,
                _ => Color.Error
            };
        }

        private Color GetRankColor(int rank)
        {
            return rank switch
            {
                1 => Color.Success,
                2 => Color.Info,
                3 => Color.Warning,
                _ => Color.Default
            };
        }
    }
}
