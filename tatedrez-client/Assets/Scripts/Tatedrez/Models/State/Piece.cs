
using System;

namespace Tatedrez.Models
{
    public class Piece
    {
        public string PieceType;
        public readonly Guid Guid;
        public int Owner;

        public Piece(int owner)
        {
            this.Guid = Guid.NewGuid();
            this.Owner = owner;
        }

        public override int GetHashCode()
        {
            return this.Guid.GetHashCode();
        }
    }
}
