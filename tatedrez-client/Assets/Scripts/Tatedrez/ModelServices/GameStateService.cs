using Tatedrez.Models;

namespace Tatedrez.ModelServices
{
    public class GameStateService
    {
        private GameState gameState;

        public bool IsGameActive =>
            this.gameState.Stage != Stage.End
            && this.gameState.Stage != Stage.Unknown;

        public void SetData(GameState dataStateData)
        {
            this.gameState = dataStateData;
        }
    }
}