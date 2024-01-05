using System.Threading.Tasks;
using Tatedrez.Models;

namespace Tatedrez
{
    public interface IMoveFetcher
    {
        Task<PlacementMove> GetMovePiecePlacement();
        Task<MovementMove> GetMovePieceMovement();
    }
}