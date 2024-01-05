using System.Collections.Generic;

namespace Tatedrez.Models
{
    public class Board
    {
        public BoardCoords BoardSize;
        public Dictionary<int, Piece> PiecesByCoordinates = new();
    }
}