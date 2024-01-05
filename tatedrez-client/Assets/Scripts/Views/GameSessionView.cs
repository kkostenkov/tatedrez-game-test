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

        private GameSessionDataService sessionDataService;

        public async Task Build(GameSessionDataService sessionDataService)
        {
            this.sessionDataService = sessionDataService;
            var board = sessionDataService.BoardService;
            await this.boardView.BuildBoardAsync(board);
            
            for (int i = 0; i < sessionDataService.GetPlayersCount; i++) {
                await playerViews[i].Initialize(sessionDataService.GetPlayer(i));
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
    }
}