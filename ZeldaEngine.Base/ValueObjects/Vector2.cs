using System;

namespace ZeldaEngine.Base.ValueObjects
{
    public class Vector2 : IEquatable<Vector2>
    {
        public float X { get; set; }

        public float Y { get; set; }

        public Vector2()
        {
            X = Y = 0;
        }

        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }


        public bool Equals(Vector2 other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Y.Equals(other.Y) && X.Equals(other.X);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Vector2) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Y.GetHashCode()*397) ^ X.GetHashCode();
            }
        }

        public static bool operator ==(Vector2 left, Vector2 right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Vector2 left, Vector2 right)
        {
            return !Equals(left, right);
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

        public static float Distance(Vector2 v1, Vector2 v2)
        {
            float num1 = v1.X - v2.X;
            float num2 = v1.Y - v2.Y;
            return (float) Math.Sqrt(num1 * num1 + num2 * num2);
        }

        public override string ToString()
        {
            return string.Format("X: {0}, Y: {1}", X, Y);
        }
    }
}
