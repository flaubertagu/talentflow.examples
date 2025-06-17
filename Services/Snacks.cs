using MudBlazor;

namespace TalentFlow.Demo.Services
{
    public class Snacks(ISnackbar Snackbar)
    {
        public ISnackbar Snackbar { get; } = Snackbar;

        public void Simple(string message)
        {
            if (string.IsNullOrEmpty(message)) return;
            Snackbar.Configuration.PositionClass = Defaults.Classes.Position.BottomRight;
            Snackbar.Configuration.PreventDuplicates = false;
            Snackbar.Add(message, Severity.Info);
        }

        public void Success(string message)
        {
            if (string.IsNullOrEmpty(message)) return;
            Snackbar.Configuration.PositionClass = Defaults.Classes.Position.BottomRight;
            Snackbar.Configuration.PreventDuplicates = false;
            Snackbar.Add(message, Severity.Success);
        }

        public void Warning(string message)
        {
            if (string.IsNullOrEmpty(message)) return;
            Snackbar.Configuration.PositionClass = Defaults.Classes.Position.BottomRight;
            Snackbar.Configuration.PreventDuplicates = false;
            Snackbar.Add(message, Severity.Warning);
        }

        public void Info(string message)
        {
            if (string.IsNullOrEmpty(message)) return;
            Snackbar.Configuration.PositionClass = Defaults.Classes.Position.BottomRight;
            Snackbar.Configuration.PreventDuplicates = false;
            Snackbar.Add(message, Severity.Info);
        }

        public void Error(string message)
        {
            if (string.IsNullOrEmpty(message)) return;
            Snackbar.Configuration.PositionClass = Defaults.Classes.Position.BottomRight;
            Snackbar.Configuration.PreventDuplicates = false;
            Snackbar.Add(message, Severity.Error);
        }
    }
}
