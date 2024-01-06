using System;
using System.Collections.Generic;
using System.Linq;
using Tatedrez.Models;
using Tatedrez.ModelServices;

namespace Tatedrez.Rules
{
    public class RookRulesHolder : IPieceRulesHolder
    {
        public bool ValidateMove(MovementMove move, IBoardInfoService board)
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

        public bool HasLegitMoves(BoardCoords position, IBoardInfoService board)
        {
            var boardMovesEnumerator = AnonymousMovesGenerator.GetBoardMoves(position, MoveTemplates, board.GetSize().X, board);
            foreach (var moveCoords in boardMovesEnumerator) {
                if (ValidateMove(new MovementMove() { From = position, To = moveCoords }, board)) {
                    return true;
                }
            }
            return false;
        }

        public IEnumerable<BoardCoords> GetLegitMovementDestinations(BoardCoords position, IBoardInfoService board)
        {
            return AnonymousMovesGenerator.GetBoardMoves(position, MoveTemplates, board.GetSize().X, board);
        }

        private static readonly BoardCoords[] MoveTemplates = {
            new BoardCoords(1, 0),
            new BoardCoords(0, 1),
            new BoardCoords(-1, 0),
            new BoardCoords(0, -1),
        };
    }
}