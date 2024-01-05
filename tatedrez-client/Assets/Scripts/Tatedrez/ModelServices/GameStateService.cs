using Tatedrez.Models;

namespace Tatedrez.ModelServices
{
    public class GameStateService
    {
        private readonly GameState gameState;

        public bool IsGameActive =>
            this.gameState.Stage != Stage.End
            && this.gameState.Stage != Stage.Unknown;

        public GameStateService(GameState gameState)
        {
            this.gameState = gameState;
        }
    }
}