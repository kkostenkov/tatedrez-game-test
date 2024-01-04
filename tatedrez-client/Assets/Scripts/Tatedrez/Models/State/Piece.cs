
using System;

namespace Tatedrez.Models
{
    public class Piece
    {
        public string PieceType;
        public readonly Guid Guid;

        public Piece(Guid guid = default)
        {
            if (guid == default) {
                guid = Guid.NewGuid();
            }
            this.Guid = guid;
        }

        public override int GetHashCode()
        {
            return this.Guid.GetHashCode();
        }
    }
}
