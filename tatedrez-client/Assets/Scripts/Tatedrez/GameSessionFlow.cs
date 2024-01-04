using System.Threading.Tasks;
using Tatedrez.Models;

namespace Tatedrez
{
    public class GameSessionFlow
    {
        private GameSessionController gameSessionController;

        public async Task Prepare(GameSessionData sessionDataData, IBoardView boardView, IInputManger input) // TODO: resolve view by DI
        {
            this.gameSessionController = new GameSessionController(sessionDataData, boardView, input);
            await this.gameSessionController.BuildBoardAsync();
        }

        public async Task ProcessTurn()
        {
            await this.gameSessionController.Turn();
        }
    }
}