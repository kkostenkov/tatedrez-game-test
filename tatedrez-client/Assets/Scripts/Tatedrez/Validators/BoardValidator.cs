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
        
        private delegate bool TicTacToeChecker(IBoardInfoService board, ref List<BoardCoords> winSequence);
        
        public bool TryFindTickTackToe(IBoardInfoService board, out EndGameDetails endGameDetails)
        {
            List<TicTacToeChecker> checks = new List<TicTacToeChecker>() {
                HasVerticalTicTacToe,
                HasHorizontalTicTacToe,
                HasDiagonalTicTacToe
            };
            
            var winningCoords = new List<BoardCoords>();
            var hasTicTacToe = checks.Any(check => check.Invoke(board, ref winningCoords));
            if (hasTicTacToe) {
                endGameDetails = ComposeEndgameDetails(board, winningCoords);
                return true;
            }
            
            endGameDetails = null;
            return false;
        }

        private static EndGameDetails ComposeEndgameDetails(IBoardInfoService board, List<BoardCoords> winningCoords)
        {
            var firstPiece = board.PeekPiece(winningCoords[0]);
            var endGameDetails = new EndGameDetails() {
                WinnerId = firstPiece.Owner,
            };
            for (var index = 0; index < winningCoords.Count; index++) {
                endGameDetails.WinnerCords[index] = winningCoords[index];
            }

            return endGameDetails;
        }

        private static bool HasVerticalTicTacToe(IBoardInfoService board, ref List<BoardCoords> winningCoords)
        {
            winningCoords.Clear();
            var size = board.GetSize();
            for (int x = 0; x < size.X; x++) {
                var coords = new BoardCoords(x, 0);
                var firstPiece = board.PeekPiece(coords);
                if (firstPiece == null) {
                    winningCoords.Clear();
                    continue;
                }
                var firstPieceOwner = board.PeekPiece(coords).Owner;
                
                winningCoords.Add(coords);
                
                for (int y = 1; y < size.Y; y++) {
                    coords = new BoardCoords(x, y);
                    var piece = board.PeekPiece(coords);
                    if (piece == null || firstPieceOwner != piece.Owner) {
                        winningCoords.Clear();
                        break;
                    }
                    winningCoords.Add(coords);
                }
                if (winningCoords.Count == board.GetSize().Y) {
                    break;
                }
            }

            var boardSide = board.GetSize().Y;
            return winningCoords.Count == boardSide && winningCoords.Count != 0;
        }

        private static bool HasHorizontalTicTacToe(IBoardInfoService board, ref List<BoardCoords> winningCoords)
        {
            winningCoords.Clear();
            var size = board.GetSize();
            for (int y = 0; y < size.Y; y++) {
                var coords = new BoardCoords(0, y);
                var firstPiece = board.PeekPiece(coords);
                if (firstPiece == null) {
                    winningCoords.Clear();
                    continue;
                }
                var firstPieceOwner = firstPiece.Owner;
                
                winningCoords.Add(coords);
                
                for (int x = 1; x < size.X; x++) {
                    coords = new BoardCoords(x, y);
                    var piece = board.PeekPiece(coords);
                    if (piece == null || firstPieceOwner != piece.Owner) {
                        winningCoords.Clear();
                        break;
                    }
                    winningCoords.Add(coords);
                }

                if (winningCoords.Count == board.GetSize().X) {
                    break;
                }
            }

            return winningCoords.Count == board.GetSize().X && winningCoords.Count != 0;
        }

        private bool HasDiagonalTicTacToe(IBoardInfoService board, ref List<BoardCoords> winningCoords)
        {
            winningCoords.Clear();
            
            var diagonals = board.Diagonals;
            if (diagonals == null) {
                return false;
            }

            foreach (var diagonal in diagonals) {
                if (CheckDiagonal(diagonal, board, out winningCoords)) {
                    return true;
                }    
            }
            
            return false;
        }

        private bool CheckDiagonal(List<BoardCoords> diagonal, IBoardInfoService board, out List<BoardCoords> winningCoords)
        {
            winningCoords = null;
            if (diagonal == null || diagonal.Count == 0) {
                return false;
            }
            
            var hasTicTacToe = diagonal.All(coord =>
                board.PeekPiece(coord) != null
                && (board.PeekPiece(coord).Owner == board.PeekPiece(diagonal[0]).Owner));
            if (!hasTicTacToe) {
                return false;
            }

            winningCoords = new List<BoardCoords>(diagonal);
            return true;
        }

        public bool IsValidMove(IBoardInfoService board, PlacementMove move)
        {
            return !board.IsOccupied(move.To);
        }
    }
}