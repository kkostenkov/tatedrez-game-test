using System.Collections.Generic;
using Tatedrez.Models;
using Tatedrez.ModelServices;

namespace Tatedrez.Validators
{
    public interface ICommandValidator : IPlacementCommandValidator, IMovementCommandValidator
    {
        List<BoardCoords> TryFindTickTackToe(IBoardInfoService board);
    }
}