using System.Threading.Tasks;
using Tatedrez.Input;
using Tatedrez.Models;
using Tatedrez.ModelServices;

namespace Tatedrez
{
    public interface IGameSessionView
    {
        Task Build(IGameSessionDataService sessionDataService, IInputSourceCollector inputCollector);
        Task ShowGameOverScreen();
        Task VisualizeMove(PlacementMove move);
        Task VisualizeMove(MovementMove move);
        Task ShowTurn(int playerIndex);
        Task VisualizeInvalidMove(PlacementMove move);
        Task VisualizeInvalidMove(MovementMove move);
        Task VisualizeHasNoMoves(int playerIndex);
    }
}