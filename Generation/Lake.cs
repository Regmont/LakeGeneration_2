using LakeGeneration_2.Math;
using System;
using System.Collections.Generic;

namespace LakeGeneration_2
{
    internal class Lake
    {
        public int LeftBound { get; }
        public int RightBound { get; }
        public int UpBound { get; }
        public int DownBound { get; }
        public Vector2D Center { get; }
        public float Radius { get; }
        public Vector2D GeneratedCenter { get; }
        public List<Vector2D> PerimeterPoints { get; }
        public List<PolarPolynomial> PerimeterPolynomials { get; }

        private float randomCenterFactor;
        private float randomRadiusFactor;

        private const int NumberOfPerimeterPoints = 36;

        public Lake(int leftBound, int rightBound, int upBound, int downBound, float randomCenterFactor, float randomRadiusFactor)
        {
            LeftBound = leftBound;
            RightBound = rightBound;
            UpBound = upBound;
            DownBound = downBound;

            Center = new Vector2D((LeftBound + RightBound) / 2, (UpBound + DownBound) / 2);
            Radius = (RightBound - LeftBound) / 2;

            this.randomCenterFactor = randomCenterFactor;
            this.randomRadiusFactor = randomRadiusFactor;
            Random random = new Random();

            GeneratedCenter = new Vector2D(
                Center.X * (1 + (float)((random.NextDouble() * 2 - 1) * this.randomCenterFactor)),
                Center.Y * (1 + (float)((random.NextDouble() * 2 - 1) * this.randomCenterFactor))
            );

            PerimeterPoints = GeneratePerimeterPoints(NumberOfPerimeterPoints);
            PerimeterPolynomials = GeneratePerimeterPolynomials(PerimeterPoints);
        }

        private List<Vector2D> GeneratePerimeterPoints(int numberOfPoints)
        {
            float distanceToBoundary = GetDistanceToNearestBoundary();
            float circleRadius = distanceToBoundary * (1 - randomRadiusFactor);
            float maxRadiusDeviation = distanceToBoundary * randomRadiusFactor;

            List<Vector2D> points = new List<Vector2D>();
            Random random = new Random();

            for (int i = 0; i < numberOfPoints; i++)
            {
                float angle = (float)i / numberOfPoints * 2 * (float)System.Math.PI;
                float radiusDeviation = (float)((random.NextDouble() * 2 - 1) * maxRadiusDeviation);
                float currentRadius = circleRadius + radiusDeviation;

                float x = GeneratedCenter.X + currentRadius * (float)System.Math.Cos(angle);
                float y = GeneratedCenter.Y + currentRadius * (float)System.Math.Sin(angle);

                points.Add(new Vector2D(x, y));
            }

            return points;
        }

        private List<PolarPolynomial> GeneratePerimeterPolynomials(List<Vector2D> points)
        {
            var polynomials = new List<PolarPolynomial>();
            int n = points.Count;

            for (int i = 0; i < n; i++)
            {
                Vector2D p1 = points[i];
                Vector2D p2 = points[(i + 1) % n];
                Vector2D p3 = points[(i + 2) % n];

                polynomials.Add(new PolarPolynomial(GeneratedCenter, p1, p2, p3));
            }

            return polynomials;
        }

        private float GetDistanceToNearestBoundary()
        {
            float minX = System.Math.Min(
                System.Math.Abs(GeneratedCenter.X - LeftBound),
                System.Math.Abs(GeneratedCenter.X - RightBound)
            );

            float minY = System.Math.Min(
                System.Math.Abs(GeneratedCenter.Y - UpBound),
                System.Math.Abs(GeneratedCenter.Y - DownBound)
            );

            return System.Math.Min(minX, minY);
        }

        public StateType IsValid(int gridWidth, int gridHeight)
        {
            if (LeftBound < 0 || RightBound >= gridWidth || UpBound < 0 || DownBound >= gridHeight)
            {
                return StateType.Error;
            }

            if (LeftBound > RightBound || UpBound > DownBound)
            {
                return StateType.Invalid;
            }

            return StateType.Valid;
        }
    }
}
