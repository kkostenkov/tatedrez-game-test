using System.Collections.Generic;
using Tatedrez.Models;

namespace Tatedrez.ModelServices
{
    public class GameSessionDataService : IGameSessionDataService
    {
        public BoardService BoardService { get; }
        public GameStateService GameStateService { get; }
        public EndGameService EndGameService { get; }
        private readonly List<PlayerService> playerServices = new();
        private GameSessionData data;

        public int CurrentTurnNumber => this.data.CurrentTurn;

        public GameSessionDataService(BoardService boardService, GameStateService gameStateService, EndGameService endGameService)
        {
            BoardService = boardService;
            GameStateService = gameStateService;
            EndGameService = endGameService;
        }

        public void SetData(GameSessionData sessionData)
        {
            this.data = sessionData;
            BoardService.SetData(data.Board);
            GameStateService.SetData(data.State);
            EndGameService.SetData(data);
            for (var index = 0; index < data.Players.Count; index++) {
                var playerData = data.Players[index];
                if (this.playerServices.Count - 1 < index) {
                    this.playerServices.Add(new PlayerService(index));
                }
                this.playerServices[index].SetData(playerData);
            }
        }

        public int GetCurrentActivePlayerIndex()
        {
            return this.data.CurrentTurn % data.Players.Count;
        }

        public int GetPlayersCount => this.data.Players.Count;

        public PlayerService GetPlayer(int index)
        {
            return this.playerServices[index];
        }
    }
}