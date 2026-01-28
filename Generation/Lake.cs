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

        public Lake(int leftBound, int rightBound, int upBound, int downBound)
        {
            LeftBound = leftBound;
            RightBound = rightBound;
            UpBound = upBound;
            DownBound = downBound;

            Center = new Vector2D((LeftBound + RightBound) / 2, (UpBound + DownBound) / 2);
            Radius = (RightBound - LeftBound) / 2;
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
