using System.Collections.Generic;
using Tatedrez.Models;
using Tatedrez.ModelServices;

namespace Tatedrez
{
    public interface IMovesGenerator
    {
        bool PlayerHasMoves(int activePlayerIndex);
        bool HasMoves(Piece piece, BoardCoords position, IBoardInfoService board);
        IEnumerable<BoardCoords> GetLegitMovementDestinations(string pieceType, BoardCoords position, IBoardInfoService board);
    }
}