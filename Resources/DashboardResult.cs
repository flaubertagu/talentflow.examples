using MudBlazor;

namespace TalentFlow.Demo.Resources
{
    public static class DashboardResult
    {
        public static int RecommandationRating(int rating)
        {
            switch (rating)
            {
                case 0: return 5;
                case 1: return 4;
                case 2: return 3;
                case 3: return 2;
                case 4: return 1;
                default: return 0;
            }
        }

        public static Color RecommandationColor(int rating)
        {
            switch (rating)
            {
                case 0: return Color.Error;
                case 1: return Color.Primary;
                case 2: return Color.Warning;
                case 3: return Color.Success;
                case 4: return Color.Success;
                default: return Color.Default;
            }
        }

        public static int GetGlobalRating(double ratio)
        {
            if (ratio > 0 && ratio <= 0.20)
                return 1;
            else if (ratio > 0.20 && ratio <= 0.40)
                return 2;
            else if (ratio > 0.40 && ratio <= 0.6)
                return 3;
            else if (ratio > 0.6 && ratio <= 0.8)
                return 4;
            else if (ratio > 0.8)
                return 5;

            return 0;
        }

        public static Color GetColor(double ratio)
        {
            if (ratio > 0 && ratio <= 0.20)
                return Color.Error;
            else if (ratio > 0.20 && ratio <= 0.40)
                return Color.Primary;
            else if (ratio > 0.40 && ratio <= 0.6)
                return Color.Warning;
            else if (ratio > 0.6 && ratio <= 0.8)
                return Color.Success;
            else if (ratio > 0.8)
                return Color.Success;

            return Color.Default;
        }
    }
}
