using System;
using Tatedrez.Models;
using Tatedrez.ModelServices;

namespace Tatedrez
{
    public class BishopRulesHolder : IPieceRulesHolder
    {
        public bool ValidateMove(MovementMove move, IBoardInfoService board)
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
            var squaresCount = maxX - minX;
            var x = minX;
            var y = minY;
            for (int i = 0; i < squaresCount; i++, x++, y++) {
                var coords = new BoardCoords(x, y);
                var piece = board.PeekPiece(coords);
                if (piece != null && piece.Guid != move.PieceGuid) {
                    return false;
                }
            }

            return true;
        }
    }
}