using System;
using System.Linq;
using Tatedrez.Models;
using Tatedrez.ModelServices;

namespace Tatedrez
{
    public class MovementValidator
    {
        public bool IsValidMove(IBoardInfoService board, MovementMove move)
        {
            if (board.IsOccupied(move.To)) {
                return false;
            }

            var movingPiece = board.PeekPiece(move.From);
            if (movingPiece == null || movingPiece.Owner != move.PlayerIndex) {
                return false;
            }

            switch (movingPiece.PieceType) {
                case Constants.Rook:
                    return ValidateRook(board, move);
                case Constants.Bishop:
                    return ValidateBishop(board, move);
                default:
                    throw new System.ArgumentException(movingPiece.PieceType);
            }
        }

        private bool ValidateBishop(IBoardInfoService board, MovementMove move)
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

        private bool ValidateRook(IBoardInfoService board, MovementMove move)
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
                var min = Math.Min(from.X, to.X);
                var max = Math.Max(from.X, to.X);
                var range = Enumerable.Range(min, max);
                foreach (var number in range) {
                    var coords = new BoardCoords(number, from.X);
                    var piece = board.PeekPiece(coords);
                    if (piece != null && piece.Guid != move.PieceGuid) {
                        return false;
                    }
                }
            }
            else if (isTheSameColumn) {
                var min = Math.Min(from.Y, to.Y);
                var max = Math.Max(from.Y, to.Y);
                var range = Enumerable.Range(min, max);
                foreach (var number in range) {
                    var coords = new BoardCoords(from.X, number);
                    var piece = board.PeekPiece(coords);
                    if (piece != null && piece.Guid != move.PieceGuid) {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}