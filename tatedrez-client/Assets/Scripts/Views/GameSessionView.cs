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
            return sessionInfoView.DisplayGameOver();
        }

        public async Task VisualizeMove(PlacementMove move)
        {
            var playerIndex = move.PlayerIndex;
            var pieceGraphicsTransform = playerViews[playerIndex].GetPieceGraphicsTransform(move.PieceGuid);
            Vector3 destination = boardView.GetWorldCoords(move.To);
            await AnimatePieceMovement(pieceGraphicsTransform, destination);
            var piece = await playerViews[playerIndex].TakePiece(move.PieceGuid);
            await boardView.PutPiece(piece, move.To);
        }

        private Task AnimatePieceMovement(Transform what, Vector3 destination)
        {
            return Task.CompletedTask;
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