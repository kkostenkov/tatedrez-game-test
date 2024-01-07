using System;
using Tatedrez.Models;
using Tatedrez.ModelServices;

namespace Tatedrez.Rules
{
    public class BishopRulesHolder : BasePieceRulesHolder, IPieceRulesHolder
    {
        protected override bool ValidatePieceMove(MovementMove move, IBoardInfoService board)
        {
            var from = move.From;
            var to = move.To;

            var minX = Math.Min(from.X, to.X);
            var minY = Math.Min(from.Y, to.Y);
            var maxX = Math.Max(from.X, to.X);
            var maxY = Math.Max(from.Y, to.Y);
            var isDiagonalMove = (maxX - minX) == (maxY - minY);
            if (!isDiagonalMove) {
                return false;
            }

            // Does jump over other pieces?
            // Check path is always left to right
            var x = minX;
            var y = move.From.X == minX ? move.From.Y : move.To.Y;

            var leftmostCheckPathCoords = new BoardCoords(x, y);
            var rightmostCheckPathCoords = new BoardCoords(maxX, move.From.X == maxX ? move.From.Y : move.To.Y);
            var isMovingUp = leftmostCheckPathCoords.Y < rightmostCheckPathCoords.Y;    

            for (int i = x; i <= maxX; i++) {
                x = i;
                var coords = new BoardCoords(x, y);
                y = isMovingUp ? y + 1 : y - 1;
                if (coords == move.From) {
                    continue;
                }
                var piece = board.PeekPiece(coords);
                if (piece != null) {
                    return false;
                }
            }

            return true;
        }

        protected override BoardCoords[] MoveTemplates => this.BishopMoveTemplates;

        protected readonly BoardCoords[] BishopMoveTemplates = new[] {
            new BoardCoords(1, 1),
            new BoardCoords(-1, 1),
            new BoardCoords(1, -1),
            new BoardCoords(-1, -1),
        };
        
        public bool HasLegitMoves(BoardCoords position, IBoardInfoService board)
        {
            var maxTemplateMovementRange = board.GetSize().X;
            return base.HasLegitMoves(position, maxTemplateMovementRange, board);
        }
    }
}