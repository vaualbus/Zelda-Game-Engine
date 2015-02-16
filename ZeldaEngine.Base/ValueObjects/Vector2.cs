using System;

namespace ZeldaEngine.Base.ValueObjects
{
    public class Vector2 : IEquatable<Vector2>
    {
        public int X { get; set; }

        public int Y { get; set; }

        public Vector2()
        {
            X = Y = 0;
        }

        public Vector2(int x, int y)
        {
            X = x;
            Y = y;
        }

        public bool Equals(Vector2 other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Vector2 && Equals((Vector2) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X*397) ^ Y;
            }
        }

        public static bool operator ==(Vector2 left, Vector2 right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Vector2 left, Vector2 right)
        {
            return !left.Equals(right);
        }

        public static Vector2 Parse(string s)
        {
            var args = s.Split(',');
            return new Vector2(int.Parse(args[0]), int.Parse(args[1]));
        }

        public bool Intersect(Vector2 playerCenterPoint)
        {
            return playerCenterPoint.X == X && playerCenterPoint.Y == Y;
        }

        public bool IsInRange(Vector2 distance)
        {
            return X <= distance.X && Y <= distance.Y;
        }

        public override string ToString()
        {
            return string.Format("X: {0}, Y: {1}", X, Y);
        }
    }
}
