using System.Collections.Generic;
using Tatedrez.Models;
using Tatedrez.ModelServices;

namespace Tatedrez.Rules
{
    public abstract class BasePieceRulesHolder
    {
        protected virtual BoardCoords[] MoveTemplates { get; }

        public virtual bool ValidateMove(MovementMove move, IBoardInfoService board)
        {
            if (board.IsOccupied(move.To)) {
                return false;
            }

            return ValidatePieceMove(move, board);
        }

        protected abstract bool ValidatePieceMove(MovementMove move, IBoardInfoService board);

        protected bool HasLegitMoves(BoardCoords position, int range, IBoardInfoService board)
        {
            var boardMovesEnumerator = AnonymousMovesGenerator.GetBoardMoves(position, MoveTemplates, range, board);
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
    }
}