using System.Collections.Generic;
using Tatedrez.Models;
using Tatedrez.ModelServices;

namespace Tatedrez
{
    public interface IPieceRulesHolder : IPieceMovesGenerator, IPieceMovesValidator
    {
    }

    public interface IPieceMovesValidator
    {
        bool ValidateMove(MovementMove move, IBoardInfoService board);
    }

    public interface IPieceMovesGenerator
    {
        IEnumerable<BoardCoords> GetLegitMovementDestinations(BoardCoords position, IBoardInfoService board);
        bool HasLegitMoves(BoardCoords position, IBoardInfoService board);
    }
}