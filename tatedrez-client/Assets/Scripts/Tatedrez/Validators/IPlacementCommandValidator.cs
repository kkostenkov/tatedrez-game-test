using Tatedrez.Models;
using Tatedrez.ModelServices;

namespace Tatedrez.Validators
{
    public interface IPlacementCommandValidator
    {
        bool IsValidMove(IBoardInfoService board, PlacementMove move);
    }
}