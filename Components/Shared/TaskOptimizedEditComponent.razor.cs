using TalentFlow.Demo.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace TalentFlow.Demo.Components.Shared
{
    public partial class TaskOptimizedEditComponent
    {
        [Parameter]
        public List<TeamMember> TeamMembers { get; set; } = [];
        [Parameter]
        public TaskDoTo? TaskSelected { get; set; }
        [Parameter]
        public EventCallback OnSelectionCanceled { get; set; }
        [Parameter]
        public EventCallback<TeamMember> OnMemberChanged { get; set; }
        private TeamMember? MemberSelected;

        private void AssigneMemberSelected(TableRowClickEventArgs<TeamMember> args)
        {
            MemberSelected = args.Item;
        }

        private async Task ValidateMemberSelected() => await OnMemberChanged.InvokeAsync(MemberSelected);
    }
}
