using Microsoft.AspNetCore.Components;
using MudBlazor;
using TalentFlow.Csharp.Models.Objects;

namespace TalentFlow.Demo.Components.Shared
{
    public partial class CandidatesDisplayComponent
    {
        [Parameter]
        public List<StrategyCandidates>? Strategies { get; set; }
        [Parameter]
        public double MinimumScore { get; set; }

        private Color GetScoreColor(double score)
        {
            return score switch
            {
                >= 0.8 => Color.Success,
                >= 0.6 => Color.Warning,
                >= 0.4 => Color.Info,
                _ => Color.Error
            };
        }
    }
}
