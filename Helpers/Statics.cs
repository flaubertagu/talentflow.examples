namespace TalentFlow.Demo.Helpers
{
    public static class Statics
    {
        public static string ShortText(int maxLenght, string? text)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            int length = text.Length;
            int shortLength;
            if (length > maxLenght)
                shortLength = maxLenght;
            else
                shortLength = length;
            string shortText = text.Substring(0, shortLength);
            if (shortLength == maxLenght)
                shortText += "...";
            return shortText;
        }
    }
}
