using System.Collections.Generic;
using System.Linq;
using Tatedrez.Models;

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

        public BoardValidator()
        {
        }

        public bool HasTickTackToe(Board board)
        {
            return HasHorizontalTicTacToe(board) || HasVerticalTicTacToe(board) || HasDiagonalTicTacToe(board);
        }

        private bool HasVerticalTicTacToe(Board board)
        {
            for (int x = 0; x < board.BoardSize.X; x++) {
                var firstPiece = board.PeekPiece(new BoardCoords(x, 0));
                if (firstPiece == null) {
                    continue;
                }

                var firstPieceOwner = board.PeekPiece(new BoardCoords(x, 0)).Owner;
                for (int y = 1; y < board.BoardSize.Y; y++) {
                    var piece = board.PeekPiece(new BoardCoords(x, y));
                    if (piece == null || firstPieceOwner != piece.Owner) {
                        break;
                    }

                    return true;
                }
            }

            return false;
        }

        private bool HasHorizontalTicTacToe(Board board)
        {
            for (int y = 0; y <= board.BoardSize.Y; y++) {
                var firstPiece = board.PeekPiece(new BoardCoords(0, y));
                if (firstPiece == null) {
                    continue;
                }

                var firstPieceOwner = firstPiece.Owner;
                for (int x = 1; x <= board.BoardSize.X; x++) {
                    var piece = board.PeekPiece(new BoardCoords(x, y));
                    if (piece == null || firstPieceOwner != piece.Owner) {
                        break;
                    }

                    return true;
                }
            }

            return false;
        }

        private bool HasDiagonalTicTacToe(Board board)
        {
            return this.diagonalOne.All(coord =>
                       board.PeekPiece(coord) != null 
                       && (board.PeekPiece(coord).Owner == board.PeekPiece(this.diagonalOne[0]).Owner))
                   || this.diagonalTwo.All(coord =>
                       board.PeekPiece(coord) != null
                       && (board.PeekPiece(coord)?.Owner == board.PeekPiece(this.diagonalTwo[0])?.Owner));
        }

        public bool IsValidMove(Board board, PlacementMove move)
        {
            var toCoords = move.To;
            var occupyingPiece = board.PeekPiece(toCoords);
            return occupyingPiece == null;
        }

        public bool IsValidMove(Board board, MovementMove move)
        {
            throw new System.NotImplementedException();
        }
    }
}