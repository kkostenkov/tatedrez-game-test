using System;
using System.Collections.Generic;
using Tatedrez.Models;

namespace Tatedrez.ModelServices
{
    public interface IBoardInfoService
    {
        bool IsOccupied(BoardCoords coords);
        Piece PeekPiece(BoardCoords coords);

        BoardCoords GetSize();
        IEnumerable<Piece> FindPieces(Func<Piece, bool> checkerFunc);
        bool HasSquare(BoardCoords coords);
    }
}