using System.Threading.Tasks;
using Tatedrez.Models;

namespace Tatedrez
{
    public class GameSessionFlow
    {
        private GameSessionController gameSessionController;
        public bool IsRunning => this.gameSessionController != null && this.gameSessionController.IsSessionRunning;

        public async Task Prepare(GameSessionData sessionDataData, IGameSessionView gameSessionView, IMoveFetcher input, IActivePlayerIndexListener playerIndexListener)
        {
            this.gameSessionController = new GameSessionController(sessionDataData, gameSessionView, input, playerIndexListener);
            await this.gameSessionController.BuildBoardAsync();
        }

        public async Task ProcessTurn()
        {
            await this.gameSessionController.Turn();
        }
    }
}