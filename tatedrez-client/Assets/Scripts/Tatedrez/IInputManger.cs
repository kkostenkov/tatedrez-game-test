using System.Threading.Tasks;
using Tatedrez.Models;

namespace Tatedrez.Views
{
    public interface IInputManger
    {
        Task<PlacementMove> GetMovePiecePlacement(int playerIndex);
        Task<MovementMove> GetMovePieceMovement(int playerIndex);
    }
}