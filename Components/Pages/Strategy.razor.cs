using TalentFlow.Demo.Components.Shared;
using TalentFlow.Demo.DataGen;
using TalentFlow.Demo.Models;
using MudBlazor;
using TalentFlow.Csharp.Models.Enums;
using TalentFlow.Csharp.Models.Objects;

namespace TalentFlow.Demo.Components.Pages
{
    public partial class Strategy
    {
        /// <summary>
        /// Allows you to retrieve the configuration status of your OpenAI settings in TalentFlow.Csharp.AI 
        /// </summary>
        #region TalentFlow.Csharp.AI status configuration
        public bool SemanticIsConfigured { get; set; }
        public void AiConfigResult(bool config) => SemanticIsConfigured = config;
        #endregion

        public IsBusy IsBusy { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            await GenerateData();
        }

        private Task GenerateData()
        {
            Generator.GetRandomSkills();
            Skills = Generator.Skills;
            return Task.CompletedTask;
        }

        #region step1 - Strategic skills
        private List<string> ActivityAreas =
        [
            "Agriculture", "Automotive", "Aerospace", "Banking", "Computer science",
            "Construction", "Education", "Energy", "Entertainment", "Finance",
            "Healthcare", "Hospitality", "Insurance", "Information Technology", "Manufacturing",
            "Media", "Mining", "Pharmaceuticals", "Public Administration", "Real Estate",
            "Retail", "Telecommunications", "Tourism", "Transportation", "Utilities", "Wholesale"
        ];
        private List<string> FilteredActivities => ActivityAreas.Where(x => x.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
        private void OnActivityClick(string item)
        {
            SelectedActivity = item;
        }

        private string searchText = string.Empty;
        public string? SelectedActivity;

        private List<Skill> Skills { get; set; } = [];

        private bool GetStrategicSkillDisable =>
            string.IsNullOrEmpty(_title) || string.IsNullOrEmpty(_type) || string.IsNullOrEmpty(_strategy) || !SemanticIsConfigured;
        private string? _title;
        private string? _type = "Strategic vision";
        private string? _strategy;

        private void OnTypeChanged(bool value) => _type = value ? "Strategic vision" : "Project";

        private async Task GetStrategicSkills()
        {
            await IsBusy.Loading();
            try
            {
                var aiSkills = ConvertToAISkill.Convert(Skills);

                var skills = await AIManager.GetStrategicSkills(new AIStrategicSkillRequest()
                {
                    Lang = SupportedLang.Fr,
                    ActivityArea = SelectedActivity!,
                    TypeOfVision = _type == "Strategic vision" ? TypeOfVision.Strategic_vision : TypeOfVision.Project,
                    TextInput = _strategy!,
                    CurrentSkills = aiSkills,
                });
                foreach (var skill in skills)
                {
                    if (skill.ComplementarySkills == null)
                        skill.ComplementarySkills = [];
                }
                StrategicSkills = ConvertToStrategicSkill.Convert(skills);
            }
            catch (Exception err)
            {
                Snacks.Error(err.Message);
            }
            await IsBusy.Loading();
        }

        private StrategicSkill? SkillSelected { get; set; }
        private void OnSkillSelected(TableRowClickEventArgs<StrategicSkill> args)
        {
            SkillSelected = args.Item;
        }

        private void ResetSkillSelection()
        {
            SkillSelected = null;
            CurrentSkillSelected = null;
            searchItem = "";
        }

        private bool UpdateEquivalentDisable => CurrentSkillSelected == null ? true : false;
        public Skill? CurrentSkillSelected { get; set; }
        private string searchItem = "";
        private bool FilterFunc1(Skill element) => FilterFunc(element, searchItem);

        private bool FilterFunc(Skill element, string searchString)
        {
            if (string.IsNullOrWhiteSpace(searchString))
                return true;
            if (element.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            return false;
        }
        private void OnCurrentSkillSelected(TableRowClickEventArgs<Skill> args)
        {
            CurrentSkillSelected = args.Item;
        }

        private void ConfirmCurrentSkillSelection()
        {
            var args = CurrentSkillSelected;
            if (args == null || SkillSelected == null) return;
            SkillSelected.Equivalent = args.Name;
            ResetSkillSelection();
            StateHasChanged();
        }
        #endregion

        #region Step 2 - Result
        public List<StrategicSkill> StrategicSkills { get; set; } = [];
        private bool _expanded = false;
        private void ShowSkillsTable() => _expanded = !_expanded;
        private bool Completed;
        private void SaveResult()
        {
            //Insert your logic to save the result if necessary
            Completed = true;
        }

        private void ResetAll()
        {
            Completed = false;
            SelectedActivity = null;
            searchText = string.Empty;
            _title = null;
            _strategy = null;
            ResetSkillSelection();
        }
        #endregion
    }
}
