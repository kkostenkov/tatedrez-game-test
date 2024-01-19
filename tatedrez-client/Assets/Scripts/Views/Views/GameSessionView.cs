using System.Threading.Tasks;
using Tatedrez.Input;
using Tatedrez.Interfaces;
using Tatedrez.Models;
using Tatedrez.ModelServices;
using UnityEngine;
using UnityEngine.UI.Extensions;

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

        [SerializeField]
        private UILineRenderer winLine;

        private IGameSessionDataService sessionDataService;
        private readonly GameViewAnimator gameViewAnimator = new GameViewAnimator();

        private void Awake()
        {
            for (var i = 0; i < this.localInputManagers.Length; i++) {
                var localInputManager = this.localInputManagers[i];
                localInputManager.SetViews(this.playerViews[i], this.boardView, i);
            }
        }

        public async Task Build(IGameSessionDataService sessionDataService)
        {
            this.sessionDataService = sessionDataService;
            var board = sessionDataService.BoardService;
            await this.boardView.BuildBoardAsync(board);
            
            for (int i = 0; i < sessionDataService.GetPlayersCount; i++) {
                var player = sessionDataService.GetPlayer(i);
                var playerView = playerViews[i]; 
                await playerView.Initialize(player);
                // inputCollector.GetInputSource(i).Bind(player);
            }
        }

        public async Task ShowGameOverScreen()
        {
            var gameEndDetails = sessionDataService.EndGameService.GetEndGameDetails();
            var winCoords = gameEndDetails.WinnerCords;
            var lineStartCoords = this.boardView.GetCanvasCoords(winCoords[0]);
            var lineEndCoords = this.boardView.GetCanvasCoords(winCoords[^1]);
            await this.gameViewAnimator.AnimateWinLine(this.winLine, lineStartCoords, lineEndCoords);
            await sessionInfoView.DisplayGameOver();
        }

        public async Task VisualizeMove(PlacementMove move)
        {
            var playerIndex = move.PlayerIndex;
            var piece = await playerViews[playerIndex].TakePiece(move.PieceGuid);
            await boardView.DrawPiece(piece, move.To);
        }

        public Task VisualizeMove(MovementMove move)
        {
            var alreadyMovedInStatePiece = sessionDataService.BoardService.PeekPiece(move.To);
            var pieceType = alreadyMovedInStatePiece.PieceType;
            return this.boardView.AnimatePieceMovement(move, pieceType);
        }

        public async Task ShowTurn(int playerIndex)
        {
            await sessionInfoView.DisplayTurnNumber(sessionDataService.CurrentTurnNumber);
            await sessionInfoView.ShowPlayerToMakeMove(playerIndex);
            foreach (var playerView in this.playerViews) {
                await playerView.DisableTurnIndicator();
            }

            await this.playerViews[playerIndex].EnableTurnIndicator();
        }

        public Task VisualizeInvalidMove(PlacementMove move)
        {
            return this.boardView.FlashRed(move.To);
        }

        public Task VisualizeInvalidMove(MovementMove move)
        {
            return this.boardView.FlashRed(move.To);
        }

        public Task VisualizeHasNoMoves(int playerIndex)
        {
            return this.boardView.FlashRed();
        }

        public void BindLocalInputForPlayer(int playerIndex, IInputSourceCollector inputCollector)
        {
            inputCollector.AddInputSource(this.localInputManagers[playerIndex], playerIndex);
        }
    }
}