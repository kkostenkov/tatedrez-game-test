using System.Collections.Generic;

namespace Tatedrez.Models
{
    public class Board
    {
        public BoardCoords BoardSize;
        public Dictionary<int, Piece> PiecesByCoordinates = new();

        public Piece GetPiece(BoardCoords coords)
        {
            var key = ToKey(coords);
            return null;
        }

        private int ToKey(BoardCoords coords)
        {
            return coords.X + coords.Y * this.BoardSize.X;
        }
    }
}