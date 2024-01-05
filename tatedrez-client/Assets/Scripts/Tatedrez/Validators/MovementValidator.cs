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
            throw new System.NotImplementedException();
        }

        private bool ValidateRook(IBoardInfoService board, MovementMove move)
        {
            var from = move.From;
            var to = move.To;
            return from.X == to.X || from.Y == to.Y;
        }
    }
}