using System.Collections.Generic;
using Tatedrez.Models;

namespace Tatedrez.ModelServices
{
    public class GameSessionDataService
    {
        public readonly BoardService BoardService;
        public readonly GameStateService GameStateService;
        public readonly EndGameService EndGameService;
        private readonly List<PlayerService> PlayerServices = new();
        private readonly GameSessionData data;

        public int CurrentTurnNumber => this.data.CurrentTurn;

        public GameSessionDataService(GameSessionData data)
        {
            this.data = data;
            this.BoardService = new BoardService(data.Board);
            this.GameStateService = new GameStateService(data.State);
            this.EndGameService = new EndGameService(BoardService, data);
            for (var index = 0; index < data.Players.Count; index++) {
                var playerData = data.Players[index];
                this.PlayerServices.Add(new PlayerService(playerData, index));
            }
        }

        public int GetCurrentActivePlayerIndex()
        {
            return this.data.CurrentTurn % data.Players.Count;
        }

        public int GetPlayersCount => this.data.Players.Count;

        public PlayerService GetPlayer(int index)
        {
            return this.PlayerServices[index];
        }
    }
}