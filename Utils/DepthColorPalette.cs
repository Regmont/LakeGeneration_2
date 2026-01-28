using System.Windows.Media;

namespace LakeGeneration_2
{
    public static class DepthColorPalette
    {
        public static Color GetColorForNormalizedDepth(float depth)
        {
            if (depth < 0 || depth > 1)
            {
                return Colors.Magenta;
            }

            if (depth >= 0.7f) return Colors.DarkBlue;
            if (depth >= 0.3f) return Colors.RoyalBlue;
            if (depth >= 0.1f) return Colors.LightBlue;
            if (depth >= 0.05f) return Colors.Khaki;

            return Colors.ForestGreen;
        }
    }
}
