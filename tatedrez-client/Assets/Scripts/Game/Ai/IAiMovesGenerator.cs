using System.Threading.Tasks;
using Tatedrez.Models;
using Tatedrez.ModelServices;

namespace Tatedrez.AI
{
    public interface IAiMovesGenerator
    {
        MovementMove GenerateMovementMove(int playerIndex, BoardService boardService, IMovesGenerator movesGenerator);
        Task<PlacementMove> GeneratePlacementMove(int playerIndex, PlayerService playerService, BoardService service);
    }
}