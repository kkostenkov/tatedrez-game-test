using System.Collections.Generic;

namespace Tatedrez.Models
{
    public class Board
    {
        public BoardCoords BoardSize;
        public Dictionary<int, Piece> PiecesByCoordinates = new();
        
        public bool IsOccupied(BoardCoords coords)
        {
            var key = ToKey(coords);
            this.PiecesByCoordinates.TryGetValue(key, out var piece);
            return piece != null;
        }
        
        public Piece PeekPiece(BoardCoords coords)
        {
            var key = ToKey(coords);
            this.PiecesByCoordinates.TryGetValue(key, out var piece);
            return piece;
        }

        public void PlacePiece(Piece piece, BoardCoords coords)
        {
            PiecesByCoordinates.Add(ToKey(coords), piece);
        }

        private int ToKey(BoardCoords coords)
        {
            return coords.X + coords.Y * this.BoardSize.X;
        }
    }
}