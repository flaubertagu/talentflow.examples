using MudBlazor;
using TalentFlow.Csharp.Models.Enums;
using TalentFlow.Csharp.Models.Objects;
using TalentFlow.Demo.Components.Shared;
using TalentFlow.Demo.DataGen;
using TalentFlow.Demo.Models;

namespace TalentFlow.Demo.Components.Pages
{
    public partial class SkillsFinder
    {
        public IsBusy IsBusy { get; set; } = new();

        /// <summary>
        /// Allows you to retrieve the configuration status of your OpenAI settings in TalentFlow.Csharp.AI 
        /// </summary>
        #region TalentFlow.Csharp.AI status configuration
        public bool SemanticIsConfigured { get; set; }
        public void AiConfigResult(bool config) => SemanticIsConfigured = config;
        #endregion

        protected override async Task OnInitializedAsync()
        {
            await GenerateData();
        }

        private async Task RegenerateData()
        {
            TasksToDo.Clear();
            CurrentSkills.Clear();
            SkillsToFind.Clear();
            SkillsFound.Clear();
            await GenerateData();
            StateHasChanged();
        }

        private Task GenerateData()
        {
            Generator.GenerateTaskToDo();
            TasksToDo = Generator.TasksToDo;
            SkillsToFind = TasksToDo.Select(x => new AISkills()
            {
                Title = x.TaskName,
                Description = x.TaskDescription,
                Skills = [],
            }).ToList();

            CurrentSkills = Generator.Skills.Select(x => new AISkill()
            {
                Name = x.Name,
                Complexity = x.Complexity,
            }).ToList();
            return Task.CompletedTask;
        }

        private List<TaskDoTo> TasksToDo { get; set; } = [];
        private List<AISkill> CurrentSkills { get; set; } = [];
        private List<AISkills> SkillsToFind { get; set; } = [];
        private List<AISkills> SkillsFound { get; set; } = [];

        private async Task SearchSkills()
        {
            try
            {
                await IsBusy.Loading();
                SkillsFound = await AIManager.AiTaskSkillsList(SkillsToFind, CurrentSkills);
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

        // Helper method for complexity color coding
        private Color GetComplexityColor(string complexity)
        {
            return complexity switch
            {
                nameof(Complexity.Basic) => Color.Success,
                nameof(Complexity.Moderate) => Color.Warning,
                nameof(Complexity.Complex) => Color.Error,
                _ => Color.Default
            };
        }
    }
}
