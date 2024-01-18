using Tatedrez.Models;
using Tatedrez.ModelServices;

namespace Tatedrez.Validators
{
    public interface IMovementCommandValidator
    {
        bool IsValidMove(IBoardInfoService board, MovementMove move);
    }
}