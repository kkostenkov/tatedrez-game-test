using Tatedrez.Models;
using Tatedrez.ModelServices;
using Tatedrez.Rules;

namespace Tatedrez.Validators
{
    public class MovementValidator : IMovementCommandValidator
    {
        private readonly PieceRulesContainer pieceRules;

        public MovementValidator(PieceRulesContainer pieceRules)
        {
            this.pieceRules = pieceRules;
        }
        
        public bool IsValidMove(IBoardInfoService board, MovementMove move)
        {
            var movingPiece = board.PeekPiece(move.From);
            if (movingPiece == null || movingPiece.Owner != move.PlayerIndex) {
                return false;
            }
            
            if (board.IsOccupied(move.To)) {
                return false;
            }

            return this.pieceRules.GetPieceRules(movingPiece.PieceType).ValidateMove(move, board);
        }
    }
}