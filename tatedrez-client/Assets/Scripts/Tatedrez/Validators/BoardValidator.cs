using System.Collections.Generic;
using System.Linq;
using Tatedrez.Models;
using Tatedrez.ModelServices;

namespace Tatedrez
{
    public class BoardValidator
    {
        private readonly List<BoardCoords> diagonalOne = new() {
            new BoardCoords(0, 0), new BoardCoords(1, 1), new BoardCoords(2, 2)
        };

        private readonly List<BoardCoords> diagonalTwo = new() {
            new BoardCoords(0, 2), new BoardCoords(1, 1), new BoardCoords(2, 0)
        };

        public bool HasTickTackToe(IBoardInfoService board)
        {
            var v = HasVerticalTicTacToe(board);
            if (v != null) {
                return true;
            }

            var h = HasHorizontalTicTacToe(board);
            if (h != null) {
                return true;
            }
            var d = HasDiagonalTicTacToe(board);
            return d;
        }

        private EndGameDetails HasVerticalTicTacToe(IBoardInfoService board)
        {
            var details = new EndGameDetails();
            var size = board.GetSize();
            for (int x = 0; x < size.X; x++) {
                var coords = new BoardCoords(x, 0);
                var firstPiece = board.PeekPiece(coords);
                if (firstPiece == null) {
                    details.Clear();
                    continue;
                }
                var firstPieceOwner = board.PeekPiece(coords).Owner;
                
                details.WinnerCords[0] = coords;
                details.WinnerId = firstPieceOwner;
                
                for (int y = 1; y < size.Y; y++) {
                    coords = new BoardCoords(x, y);
                    var piece = board.PeekPiece(coords);
                    if (piece == null || firstPieceOwner != piece.Owner) {
                        details.Clear();
                        break;
                    }
                    details.WinnerCords[y] = coords;
                }
                if (details.HasWinner) {
                    break;
                }
            }

            return details.HasWinner ? details : null;
        }

        private EndGameDetails HasHorizontalTicTacToe(IBoardInfoService board)
        {
            var details = new EndGameDetails();
            var size = board.GetSize();
            for (int y = 0; y < size.Y; y++) {
                var coords = new BoardCoords(0, y);
                var firstPiece = board.PeekPiece(coords);
                if (firstPiece == null) {
                    details.Clear();
                    continue;
                }
                var firstPieceOwner = firstPiece.Owner;
                
                details.WinnerCords[0] = coords;
                details.WinnerId = firstPieceOwner;
                
                for (int x = 1; x < size.X; x++) {
                    coords = new BoardCoords(x, y);
                    var piece = board.PeekPiece(coords);
                    if (piece == null || firstPieceOwner != piece.Owner) {
                        details.Clear();
                        break;
                    }
                    details.WinnerCords[x] = coords;
                }

                if (details.HasWinner) {
                    break;
                }
            }

            return details.HasWinner ? details : null;
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
    }
}