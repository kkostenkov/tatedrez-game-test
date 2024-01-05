using System.Collections.Generic;
using System.Linq;
using Tatedrez.Models;
using Tatedrez.ModelServices;

namespace Tatedrez
{
    public class BoardValidator
    {
        private List<BoardCoords> diagonalOne = new() {
            new BoardCoords(0, 0), new BoardCoords(1, 1), new BoardCoords(2, 2)
        };

        private List<BoardCoords> diagonalTwo = new() {
            new BoardCoords(0, 2), new BoardCoords(1, 1), new BoardCoords(2, 0)
        };

        public bool HasTickTackToe(IBoardInfoService board)
        {
            return HasHorizontalTicTacToe(board) || HasVerticalTicTacToe(board) || HasDiagonalTicTacToe(board);
        }

        private bool HasVerticalTicTacToe(IBoardInfoService board)
        {
            var size = board.GetSize();
            for (int x = 0; x < size.X; x++) {
                var firstPiece = board.PeekPiece(new BoardCoords(x, 0));
                if (firstPiece == null) {
                    continue;
                }

                var firstPieceOwner = board.PeekPiece(new BoardCoords(x, 0)).Owner;
                for (int y = 1; y < size.Y; y++) {
                    var piece = board.PeekPiece(new BoardCoords(x, y));
                    if (piece == null || firstPieceOwner != piece.Owner) {
                        break;
                    }

                    return true;
                }
            }

            return false;
        }

        private bool HasHorizontalTicTacToe(IBoardInfoService board)
        {
            var size = board.GetSize();
            for (int y = 0; y <= size.Y; y++) {
                var firstPiece = board.PeekPiece(new BoardCoords(0, y));
                if (firstPiece == null) {
                    continue;
                }

                var firstPieceOwner = firstPiece.Owner;
                for (int x = 1; x <= size.X; x++) {
                    var piece = board.PeekPiece(new BoardCoords(x, y));
                    if (piece == null || firstPieceOwner != piece.Owner) {
                        break;
                    }

                    return true;
                }
            }

            return false;
        }

        private bool HasDiagonalTicTacToe(IBoardInfoService board)
        {
            return this.diagonalOne.All(coord =>
                       board.PeekPiece(coord) != null 
                       && (board.PeekPiece(coord).Owner == board.PeekPiece(this.diagonalOne[0]).Owner))
                   || this.diagonalTwo.All(coord =>
                       board.PeekPiece(coord) != null
                       && (board.PeekPiece(coord)?.Owner == board.PeekPiece(this.diagonalTwo[0])?.Owner));
        }

        public bool IsValidMove(IBoardInfoService board, PlacementMove move)
        {
            return !board.IsOccupied(move.To);
        }

        public bool IsValidMove(IBoardInfoService board, MovementMove move)
        {
            if (board.IsOccupied(move.To)) {
                return false;
            }

            var movingPiece = board.PeekPiece(move.From);
            if (movingPiece == null || movingPiece.Owner != move.PlayerIndex) {
                return false;
            }
            return true;
        }
    }
}