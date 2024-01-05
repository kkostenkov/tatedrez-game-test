using System.Threading.Tasks;
using Tatedrez.Models;
using Tatedrez.ModelServices;
using UnityEngine;

namespace Tatedrez.Views
{
    public class GameSessionView : MonoBehaviour, IGameSessionView
    {
        [SerializeField]
        private BoardView boardView;
        [SerializeField]
        private SessionInfoView sessionInfoView;
        [SerializeField]
        private PlayerView[] playerViews;
        [SerializeField]
        private LocalInputManager[] localInputManagers;

        private GameSessionDataService sessionDataService;

        public async Task Build(GameSessionDataService sessionDataService)
        {
            this.sessionDataService = sessionDataService;
            var board = sessionDataService.BoardService;
            await this.boardView.BuildBoardAsync(board);
            
            for (int i = 0; i < sessionDataService.GetPlayersCount; i++) {
                var player = sessionDataService.GetPlayer(i);
                await playerViews[i].Initialize(player);
                localInputManagers[i].Bind(player, playerViews[i], this.boardView);
            }

            // game stage
            return;
        }

        public Task ShowGameOverScreen()
        {
            throw new System.NotImplementedException();
        }

        public Task VisualizeMove(PlacementMove move)
        {
            throw new System.NotImplementedException();
        }

        public Task VisualizeMove(MovementMove move)
        {
            throw new System.NotImplementedException();
        }

        public async Task ShowTurn(int playerIndex)
        {
            await sessionInfoView.DisplayTurnNumber(sessionDataService.CurrentTurnNumber);
            await sessionInfoView.ShowPlayerToMakeMove(playerIndex);
        }

        public Task VisualizeInvalidMove(PlacementMove move)
        {
            throw new System.NotImplementedException();
        }

        public void BindLocalInputForPlayer(int playerIndex, IInputSourceCollector inputCollector)
        {
            inputCollector.AddInputSource(this.localInputManagers[playerIndex], playerIndex);
        }
    }
}