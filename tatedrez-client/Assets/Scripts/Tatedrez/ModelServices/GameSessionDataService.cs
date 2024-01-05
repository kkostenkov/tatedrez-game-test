using Tatedrez.Models;

namespace Tatedrez.ModelServices
{
    public class GameSessionDataService
    {
        public readonly BoardService BoardService;
        public readonly GameStateService GameStateService;
        private readonly GameSessionData data;

        public GameSessionDataService(GameSessionData data)
        {
            this.data = data;
            this.BoardService = new BoardService(data.Board);
            this.GameStateService = new GameStateService(data.State);
        }

        public int GetCurrentActivePlayerIndex()
        {
            return this.data.CurrentTurn % data.Players.Count;
        }

        public int CurrentTurnNumber => this.data.CurrentTurn;
    }
}