using System.Threading.Tasks;
using Tatedrez.Models;

namespace Tatedrez.Interfaces
{
    public interface IMoveFetcher
    {
        Task<PlacementMove> GetMovePiecePlacement();
        Task<MovementMove> GetMovePieceMovement();
    }
}