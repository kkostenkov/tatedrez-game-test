using System;
using System.Collections.Generic;
using Tatedrez.Models;
using Tatedrez.ModelServices;

namespace Tatedrez.Rules
{
    public class KnightRulesHolder : IPieceRulesHolder
    {
        public bool ValidateMove(MovementMove move, IBoardInfoService board)
        {
            var horizontalMoveLength = Math.Abs(move.From.X - move.To.X);
            var verticalMoveLength = Math.Abs(move.From.Y - move.To.Y);
            var hasTwoSquaresChange = horizontalMoveLength == 2 || verticalMoveLength == 2;
            var hasOneSquareChange = horizontalMoveLength == 1 || verticalMoveLength == 1;
            return hasTwoSquaresChange && hasOneSquareChange;
        }

        public bool HasLegitMoves(BoardCoords position, IBoardInfoService board)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BoardCoords> GetLegitMovementDestinations(BoardCoords position, IBoardInfoService board)
        {
            throw new NotImplementedException();
        }
    }
}