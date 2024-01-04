using System.Threading.Tasks;
using Tatedrez.Models;
using Tatedrez.Views;

namespace Tatedrez
{
    public class GameSessionFlow
    {
        private GameSession sessionData;
        private GameSessionController gameSessionController;

        public async Task Prepare(GameSession sessionData, IBoardView boardView, IInputManger input) // TODO: resolve view by DI
        {
            this.sessionData = sessionData;
            this.gameSessionController = new GameSessionController(sessionData, boardView, input);
            await this.gameSessionController.BuildBoardAsync();
        }

        public async Task Step()
        {
            var state = sessionData.State;
            switch (state.Stage) {
                case Stage.Unknown:
                    return;
                case Stage.Placement:
                    await this.gameSessionController.PlacePieceByPlayer(sessionData.CurrentPlayerTurnIndex);
                    break;
                case Stage.Movement:
                    await this.gameSessionController.MovePieceByPlayer(sessionData.CurrentPlayerTurnIndex);
                    break;
                case Stage.End:
                    await this.gameSessionController.EndGame();
                    break;
            }
        }
    }
}