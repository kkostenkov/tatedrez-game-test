using System.Collections.Generic;
using Tatedrez.Models;
using Tatedrez.ModelServices;

namespace Tatedrez.Validators
{
    public class CommandValidator : ICommandValidator
    {
        private readonly BoardValidator boardValidator;
        private readonly MovementValidator movementValidator;

        public CommandValidator(BoardValidator boardValidator, MovementValidator movementValidator)
        {
            this.boardValidator = boardValidator;
            this.movementValidator = movementValidator;
        }
        public bool IsValidMove(IBoardInfoService board, PlacementMove move)
        {
            return this.boardValidator.IsValidMove(board, move);
        }

        public bool IsValidMove(IBoardInfoService board, MovementMove move)
        {
            return movementValidator.IsValidMove(board, move);
        }

        public List<BoardCoords> TryFindTickTackToe(IBoardInfoService board)
        {
            return this.boardValidator.TryFindTickTackToe(board);
        }
    }
}