using System;
using System.Runtime.InteropServices;

namespace ZeldaEngine.ScriptEngine.ValueObjects
{
    public interface IVector2
    {
        int X { get; set; }

        int Y { get; set; }

        bool Equals(Vector2 other);

        bool Equals(object obj);
    }

    [ClassInterface(ClassInterfaceType.None)]
    [Guid("61F7FDBE-3A81-403E-909F-7FFAAB6C80C9")]
    public class Vector2 : IEquatable<Vector2>, IVector2
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
    }
}
