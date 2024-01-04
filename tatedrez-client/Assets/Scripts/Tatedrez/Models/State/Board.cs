
using System.Collections.Generic;

namespace Tatedrez.Models
{
    public class Board
    {
        public BoardCoords BoardSize;
        public List<Piece> Pieces = new();
    }
}