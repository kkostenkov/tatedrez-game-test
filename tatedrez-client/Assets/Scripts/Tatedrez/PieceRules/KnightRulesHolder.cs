using System;
using Tatedrez.Models;
using Tatedrez.ModelServices;
using Tatedrez.Validators;

namespace Tatedrez.Rules
{
    public class KnightRulesHolder : BasePieceRulesHolder, IPieceRulesHolder
    {
        protected override bool ValidatePieceMove(MovementMove move, IBoardInfoService board)
        {
            var horizontalMoveLength = Math.Abs(move.From.X - move.To.X);
            var verticalMoveLength = Math.Abs(move.From.Y - move.To.Y);
            var hasTwoSquaresChange = horizontalMoveLength == 2 || verticalMoveLength == 2;
            var hasOneSquareChange = horizontalMoveLength == 1 || verticalMoveLength == 1;
            return hasTwoSquaresChange && hasOneSquareChange;
        }

        protected override BoardCoords[] MoveTemplates => this.KnightMoveTemplates;

        protected readonly BoardCoords[] KnightMoveTemplates = new[] {
            new BoardCoords(2, 1),
            new BoardCoords(2, -1),
            new BoardCoords(-2, 1),
            new BoardCoords(-2, -1),
            new BoardCoords(1, -2),
            new BoardCoords(1, 2),
            new BoardCoords(-1, -2),
            new BoardCoords(-1, 2),
        };

        public bool HasMoves(BoardCoords position, IBoardInfoService board)
        {
            var maxTemplateMovementRange = 1;
            return base.HasMoves(position, maxTemplateMovementRange, board);
        }
    }
}