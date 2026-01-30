namespace LakeGeneration_2
{
    internal class Vector2D
    {
        public float X { get; }
        public float Y { get; }

        public Vector2D(float x, float y)
        {
            X = x;
            Y = y;
        }

        public Vector2D() : this(0, 0) { }

        public static Vector2D operator -(Vector2D a, Vector2D b)
        {
            return new Vector2D(a.X - b.X, a.Y - b.Y);
        }

        public float Length()
        {
            return (float)System.Math.Sqrt(X * X + Y * Y);
        }
    }
}
