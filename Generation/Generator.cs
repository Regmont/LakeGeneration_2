using LakeGeneration_2.Math;
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
            FillLake(lake);

            //Debug
            FillSquare(5, lake.GeneratedCenter, 0.0f);
            FillSquare(5, lake.Center, 0.5f);

            foreach (var point in lake.PerimeterPoints)
            {
                FillSquare(3, point, 0.2f);
            }

            DrawLakeOutline(lake, 3, 0.1f);
        }

        private void FillLake(Lake lake)
        {
            int n = lake.PerimeterPoints.Count;

            for (int i = lake.UpBound; i < lake.DownBound; i++)
            {
                for (int j = lake.LeftBound; j < lake.RightBound; j++)
                {
                    float dx = j + 0.5f - lake.GeneratedCenter.X;
                    float dy = i + 0.5f - lake.GeneratedCenter.Y;

                    float angle = (float)System.Math.Atan2(dy, dx);

                    if (angle < 0)
                    {
                        angle += 2 * (float)System.Math.PI;
                    }

                    int segmentIndex = FindSegmentIndex(lake, angle);
                    PolarPolynomial polynomial = lake.PerimeterPolynomials[segmentIndex];
                    float expectedRadius = polynomial.RadiusAt(angle);
                    float actualRadius = (float)System.Math.Sqrt(dx * dx + dy * dy);

                    if (actualRadius <= expectedRadius)
                    {
                        grid[j, i] = 1.0f;
                    }
                }
            }
        }

        private int FindSegmentIndex(Lake lake, float angle)
        {
            int n = lake.PerimeterPoints.Count;

            for (int k = 0; k < n; k++)
            {
                Vector2D currentPoint = lake.PerimeterPoints[k];
                Vector2D nextPoint = lake.PerimeterPoints[(k + 1) % n];

                float currentAngle = GetAngle(lake.GeneratedCenter, currentPoint);
                float nextAngle = GetAngle(lake.GeneratedCenter, nextPoint);

                if (IsAngleInSegment(angle, currentAngle, nextAngle))
                {
                    return k;
                }
            }

            return 0;
        }

        private float GetAngle(Vector2D center, Vector2D point)
        {
            Vector2D v = point - center;
            float angle = (float)System.Math.Atan2(v.Y, v.X);

            if (angle < 0)
            {
                angle += 2 * (float)System.Math.PI;
            }

            return angle;
        }

        private bool IsAngleInSegment(float angle, float start, float end)
        {
            if (end < start)
            {
                return angle >= start || angle < end;
            }

            return angle >= start && angle < end;
        }

        //Debug methods
        private void FillSquare(int size, Vector2D center, float depth)
        {
            int centerVoxelX = (int)System.Math.Floor(center.X);
            int centerVoxelY = (int)System.Math.Floor(center.Y);

            for (int i = -size; i <= size; i++)
            {
                for (int j = -size; j <= size; j++)
                {
                    int voxelX = centerVoxelX + j;
                    int voxelY = centerVoxelY + i;

                    grid[voxelX, voxelY] = depth;
                }
            }
        }

        private void DrawLakeOutline(Lake lake, int size, float depth)
        {
            for (int j = lake.LeftBound; j < lake.RightBound; j++)
            {
                for (int k = -size; k <= size; k++)
                {
                    int topY = lake.UpBound + k;
                    int bottomY = lake.DownBound - 1 + k;

                    if (topY >= 0 && topY < grid.GetLength(1))
                    {
                        grid[j, topY] = depth;
                    }

                    if (bottomY >= 0 && bottomY < grid.GetLength(1))
                    {
                        grid[j, bottomY] = depth;
                    }
                }
            }

            for (int i = lake.UpBound; i < lake.DownBound; i++)
            {
                for (int k = -size; k <= size; k++)
                {
                    int leftX = lake.LeftBound + k;
                    int rightX = lake.RightBound - 1 + k;

                    if (leftX >= 0 && leftX < grid.GetLength(0))
                    {
                        grid[leftX, i] = depth;
                    }

                    if (rightX >= 0 && rightX < grid.GetLength(0))
                    {
                        grid[rightX, i] = depth;
                    }
                }
            }
        }
    }
}
