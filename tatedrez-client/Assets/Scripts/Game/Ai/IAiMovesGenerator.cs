using System.Threading.Tasks;
using Tatedrez.Models;
using Tatedrez.ModelServices;

namespace Tatedrez.AI
{
    public interface IAiMovesGenerator
    {
        MovementMove GenerateMovementMove(int playerIndex, IBoardInfoService boardService, IMovesGenerator movesGenerator);
        Task<PlacementMove> GeneratePlacementMove(int playerIndex, PlayerService playerService, IBoardInfoService service);
    }
}