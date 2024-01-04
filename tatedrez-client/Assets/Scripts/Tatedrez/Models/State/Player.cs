using System;
using System.Collections.Generic;
using System.Linq;

namespace Tatedrez.Models
{
    public class Player
    {
        public LinkedList<Piece> UnusedPieces = new();

        public Piece DropPiece(Guid guidToDrop)
        {
            var pieceToDrop = this.UnusedPieces.FirstOrDefault(p => p.Guid == guidToDrop);
            if (pieceToDrop == null) {
                return null;
            }

            this.UnusedPieces.Remove(pieceToDrop);
            return pieceToDrop;
        }
    }
}