using System.Threading.Tasks;
using Tatedrez.Models;

namespace Tatedrez
{
    internal class LocalInputManager : IMoveFetcher
    {
        public Task<PlacementMove> GetMovePiecePlacement()
        {
            throw new System.NotImplementedException();
        }

        public Task<MovementMove> GetMovePieceMovement()
        {
            throw new System.NotImplementedException();
        }
    }
}