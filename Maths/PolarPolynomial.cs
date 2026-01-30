namespace LakeGeneration_2.Math
{
    internal class PolarPolynomial
    {
        public float A { get; }
        public float B { get; }
        public float C { get; }

        public PolarPolynomial(Vector2D center, Vector2D p1, Vector2D p2, Vector2D p3)
        {
            Vector2D polar1 = GetPolar(p1 - center);
            Vector2D polar2 = GetPolar(p2 - center);
            Vector2D polar3 = GetPolar(p3 - center);

            float r1 = polar1.X;
            float θ1 = polar1.Y;
            float r2 = polar2.X;
            float θ2 = polar2.Y;
            float r3 = polar3.X;
            float θ3 = polar3.Y;

            float denom = (θ1 - θ2) * (θ1 - θ3) * (θ2 - θ3);

            A = ((θ2 - θ3) * r1 + (θ3 - θ1) * r2 + (θ1 - θ2) * r3) / denom;
            B = ((θ3 * θ3 - θ2 * θ2) * r1 + (θ1 * θ1 - θ3 * θ3) * r2 + (θ2 * θ2 - θ1 * θ1) * r3) / denom;
            C = (θ2 * θ3 * (θ2 - θ3) * r1 + θ3 * θ1 * (θ3 - θ1) * r2 + θ1 * θ2 * (θ1 - θ2) * r3) / denom;
        }

        public float RadiusAt(float angle)
        {
            return A * angle * angle + B * angle + C;
        }

        private Vector2D GetPolar(Vector2D v)
        {
            float r = (float)System.Math.Sqrt(v.X * v.X + v.Y * v.Y);
            float θ = (float)System.Math.Atan2(v.Y, v.X);

            if (θ < 0)
            {
                θ += 2 * (float)System.Math.PI;
            }

            return new Vector2D(r, θ);
        }
    }
}
