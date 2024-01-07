using System;

namespace Tatedrez.Models
{
    public struct BoardCoords
    {
        public static BoardCoords Invalid = new BoardCoords(-1, -1);

        public int X;

        public int Y;

        public BoardCoords(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public override string ToString()
        {
            return $"{X}:{Y}";
        }

        public static bool operator ==(BoardCoords obj1, BoardCoords obj2)
        {
            return (obj1.X == obj2.X
                    && obj1.Y == obj2.Y);
        }

        public static bool operator !=(BoardCoords obj1, BoardCoords obj2)
        {
            return !(obj1.X == obj2.X
                     && obj1.Y == obj2.Y);
        }

        public bool Equals(BoardCoords other)
        {
            return this.X == other.X && this.Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            return obj is BoardCoords other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.X, this.Y);
        }
    }
}