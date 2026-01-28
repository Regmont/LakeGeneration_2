using System;

namespace LakeGeneration_2
{
    internal class Generator
    {
        private float[,] grid;

        public Generator(int gridWidth, int gridHeight, Lake lake)
        {
            grid = new float[gridWidth, gridHeight];

            if (lake == null)
            {
                throw new ArgumentNullException("No lake is provided");
            }

            StateType lakeIsValid = lake.IsValid(gridWidth, gridHeight);

            switch (lakeIsValid)
            {
                case StateType.Error:
                    throw new Exception("Lake is out of grid bounds");
                case StateType.Invalid:
                    throw new Exception("Lake has incorrect bounds");
                case StateType.Valid:
                    break;
                default:
                    throw new Exception("Unkown state of the lake");
            }

            GenerateLake(lake);
        }

        public float[,] GetGrid()
        {
            return grid;
        }

        private void GenerateLake(Lake lake)
        {
            float radiusSquared = lake.Radius * lake.Radius;
            float centerX = lake.Center.X;
            float centerY = lake.Center.Y;

            for (int i = lake.UpBound; i < lake.DownBound; i++)
            {
                for (int j = lake.LeftBound; j < lake.RightBound; j++)
                {
                    float dx = j + 0.5f - centerX;
                    float dy = i + 0.5f - centerY;

                    if (dx * dx + dy * dy <= radiusSquared)
                    {
                        grid[j, i] = 1.0f;
                    }
                }
            }
        }
    }
}
