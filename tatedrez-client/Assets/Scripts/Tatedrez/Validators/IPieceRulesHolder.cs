using Tatedrez.Models;
using Tatedrez.ModelServices;

namespace Tatedrez
{
    public interface IPieceRulesHolder
    {
        bool ValidateMove(MovementMove move, IBoardInfoService board);
    }
}