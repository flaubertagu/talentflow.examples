using MudBlazor;
using System.Text.RegularExpressions;

namespace TalentFlow.Demo.Resources
{
    public static class CustomTextFormatter
    {
        public static List<FormattedLine> ConvertToLines(string input)
        {
            var result = new List<FormattedLine>();
            if (string.IsNullOrWhiteSpace(input)) return result;

            var lines = Regex.Split(input, @"\r\n|\r|\n");

            foreach (var line in lines)
            {
                if (line.StartsWith("#### "))
                {
                    var titleText = line.Substring(5).Replace("**", string.Empty).Trim();
                    result.Add(new FormattedLine
                    {
                        Html = $"<span style='color:#f44336; font-weight:bold;'>{titleText}</span>",
                        Typography = Typo.h4
                    });
                }
                else if (line.StartsWith("### "))
                {
                    var subtitleText = line.Substring(4).Trim();
                    result.Add(new FormattedLine
                    {
                        Html = $"<span style='color:#e95420; font-weight:bold;'>{subtitleText}</span>",
                        Typography = Typo.h5
                    });
                }
                else
                {
                    var formatted = Regex.Replace(line, @"\*\*\*\s*(.+?)\s*\*\*\*", "<span style=\"color:#007ACC;font-weight:bold;\">$1</span>");
                    formatted = Regex.Replace(formatted, @"\*\*\s*(.+?)\s*\*\*", "<strong>$1</strong>") + "<br />";
                    if (formatted.Contains("**"))
                        formatted = formatted.Replace("**", string.Empty);
                    result.Add(new FormattedLine { Html = formatted, Typography = Typo.body1 });
                }
            }

            return result;
        }
    }
    public class FormattedLine
    {
        public required string Html { get; set; }
        public required Typo Typography { get; set; }
    }
}
