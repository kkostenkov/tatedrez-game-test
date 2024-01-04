using System.Threading.Tasks;
using Tatedrez.Models;

namespace Tatedrez
{
    public interface IPlayerInputReceiver
    {
        Task<PlacementMove> GetMovePiecePlacement();
        Task<MovementMove> GetMovePieceMovement();
    }
}