using System;

namespace Assets.Scripts
{
    [Serializable]
    public class Point
    {
        public int X, Y;

        public Point(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public static Point operator +(Point a, Point b)
        {
            return new Point(a.X + b.X, a.Y + b.Y);
        }

        public static Point operator -(Point a, Point b)
        {
            return new Point(a.X - b.X, a.Y - b.Y);
        }

        public override string ToString()
        {
            return $"{X}:{Y}";
        }
    }
}