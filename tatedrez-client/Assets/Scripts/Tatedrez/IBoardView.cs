using System.Threading.Tasks;
using Tatedrez.Models;

namespace Tatedrez
{
    public interface IBoardView
    {
        Task Build(GameSessionData sessionData);
        Task ShowGameOverScreen();
        Task VisualizeMove(PlacementMove move);
        Task VisualizeMove(MovementMove move);
        Task ShowTurn(int playerIndex);
    }
}