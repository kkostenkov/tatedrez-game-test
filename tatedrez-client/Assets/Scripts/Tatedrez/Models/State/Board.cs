using System.Collections.Generic;

namespace Tatedrez.Models
{
    public class Board
    {
        public BoardCoords BoardSize;
        public Dictionary<int, Piece> PiecesByCoordinates = new();

        public Piece PeekPiece(BoardCoords coords)
        {
            var key = ToKey(coords);
            return this.PiecesByCoordinates[key];
        }

        private int ToKey(BoardCoords coords)
        {
            return coords.X + coords.Y * this.BoardSize.X;
        }

        public void PlacePiece(Piece piece, BoardCoords coords)
        {
            PiecesByCoordinates.Add(ToKey(coords), piece);
        }
    }
}