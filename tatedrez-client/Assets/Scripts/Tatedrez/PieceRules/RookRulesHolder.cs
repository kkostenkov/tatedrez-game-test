using System;
using System.Linq;
using Tatedrez.Models;
using Tatedrez.ModelServices;

namespace Tatedrez.Rules
{
    public class RookRulesHolder : BasePieceRulesHolder, IPieceRulesHolder
    {
        protected override bool ValidatePieceMove(MovementMove move, IBoardInfoService board)
        {
            var from = move.From;
            var to = move.To;
            var isTheSameRow = from.Y == to.Y;
            var isTheSameColumn = from.X == to.X;
            var isStraightLine = isTheSameRow || isTheSameColumn;
            if (!isStraightLine) {
                return false;
            }

            // Does jump over pieces?
            if (isTheSameRow) {
                var y = from.Y;
                var minX = Math.Min(from.X, to.X);
                var maxX = Math.Max(from.X, to.X);
                
                var range = Enumerable.Range(minX, maxX - minX + 1);
                foreach (var number in range) {
                    var coords = new BoardCoords(number, y);
                    if (coords == move.From) {
                        continue;
                    }
                    var piece = board.PeekPiece(coords);
                    if (piece != null) {
                        return false;
                    }
                }
            }
            else if (isTheSameColumn) {
                var x = from.X;
                var minY = Math.Min(from.Y, to.Y);
                var maxY = Math.Max(from.Y, to.Y);
                var range = Enumerable.Range(minY, maxY - minY + 1);
                foreach (var number in range) {
                    var coords = new BoardCoords(x, number);
                    if (coords == move.From) {
                        continue;
                    }
                    var piece = board.PeekPiece(coords);
                    if (piece != null) {
                        return false;
                    }
                }
            }

            return true;
        }
        
        protected override BoardCoords[] MoveTemplates => this.RookMoveTemplates;

        protected readonly BoardCoords[] RookMoveTemplates = new[] {
            new BoardCoords(1, 0),
            new BoardCoords(0, 1),
            new BoardCoords(-1, 0),
            new BoardCoords(0, -1),
        };

        public bool HasLegitMoves(BoardCoords position, IBoardInfoService board)
        {
            var maxTemplateMovementRange = board.GetSize().X;
            return base.HasLegitMoves(position, maxTemplateMovementRange, board);
        }
    }
}