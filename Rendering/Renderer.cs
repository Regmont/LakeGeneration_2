using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LakeGeneration_2
{
    public class Renderer
    {
        private WriteableBitmap bitmap;

        public WriteableBitmap InitializeBitmap(int width, int height)
        {
            bitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Bgra32, null);

            return bitmap;
        }

        public void DrawDepthGrid(float[,] depthGrid)
        {
            if (bitmap == null) throw new ArgumentNullException("bitmap");
            if (depthGrid == null) throw new ArgumentNullException("depthGrid");

            int width = bitmap.PixelWidth;
            int height = bitmap.PixelHeight;

            bitmap.Lock();

            try
            {
                int[] pixels = new int[width * height];

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        float depth = depthGrid[x, y];
                        var color = DepthColorPalette.GetColorForNormalizedDepth(depth);

                        pixels[y * width + x] = (color.A << 24) | (color.R << 16) | (color.G << 8) | color.B;
                    }
                }

                bitmap.WritePixels(new Int32Rect(0, 0, width, height), pixels, width * 4, 0);
            }
            finally
            {
                bitmap.Unlock();
            }
        }
    }
}
