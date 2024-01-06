using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tatedrez.Models;
using Tatedrez.ModelServices;

namespace Tatedrez
{
    public class GameSessionController
    {
        private readonly GameSessionData sessionData;
        private readonly IGameSessionView gameSessionView;
        private readonly IMoveFetcher input;
        private readonly IActivePlayerIndexListener playerIndexListener;
        private readonly BoardValidator boardValidator;
        
        private readonly GameSessionDataService sessionDataService;
        private readonly BoardService boardService;

        public bool IsSessionRunning => sessionDataService.GameStateService.IsGameActive;

        public GameSessionController(GameSessionData sessionData, IGameSessionView gameSessionView, IMoveFetcher input, IActivePlayerIndexListener playerIndexListener)
        {
            this.sessionData = sessionData;
            this.sessionDataService = new GameSessionDataService(sessionData);
            this.boardService = this.sessionDataService.BoardService;
            this.gameSessionView = gameSessionView;
            this.input = input;
            this.playerIndexListener = playerIndexListener;
            this.boardValidator = new BoardValidator();
        }

        public Task Turn()
        {
            var playerTurnIndex = this.sessionDataService.GetCurrentActivePlayerIndex();
            var state = this.sessionData.State;
            switch (state.Stage) {
                case Stage.Unknown:
                    return Task.CompletedTask;
                case Stage.Placement:
                    return PlacePieceByPlayer(playerTurnIndex);
                case Stage.Movement:
                    return MovePieceByPlayer(playerTurnIndex);
                case Stage.End:
                    return EndGame();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public Task BuildBoardAsync()
        {
            var tasks = new List<Task> {
                this.gameSessionView.Build(this.sessionDataService),
                this.gameSessionView.ShowTurn(this.sessionDataService.GetCurrentActivePlayerIndex())
            };
            return Task.WhenAll(tasks);
        }

        private async Task PlacePieceByPlayer(int playerIndex)
        {
            await this.gameSessionView.ShowTurn(playerIndex);
            playerIndexListener.SetActivePlayer(playerIndex);
            var move = await input.GetMovePiecePlacement();
            
            if (IsInvalidMove(move)) {
                await this.gameSessionView.VisualizeInvalidMove(move);
                return;
            }
            
            ApplyStateChange(move);
            
            await this.gameSessionView.VisualizeMove(move);
        }

        private void ApplyStateChange(PlacementMove move)
        {
            var playerIndex = move.PlayerIndex;
            var player = this.sessionData.Players[playerIndex];
            var piece = player.DropPiece(move.PieceGuid);
            this.boardService.PlacePiece(piece, move.To);
            this.sessionData.CurrentTurn++;
            TryUpdateGameStage();
        }

        private bool IsInvalidMove(PlacementMove move)
        {
            if (this.sessionDataService.GetCurrentActivePlayerIndex() != move.PlayerIndex) {
                return true;
            }
            
            if (!this.boardValidator.IsValidMove(this.boardService, move)) {
                return true;
            }
            
            return false;
        }

        private async Task MovePieceByPlayer(int playerIndex)  
        {
            await this.gameSessionView.ShowTurn(playerIndex);
            // validate if there are available moves
            
            playerIndexListener.SetActivePlayer(playerIndex);
            var move = await input.GetMovePieceMovement();
            if (IsInvalidMove(move)) {
                await this.gameSessionView.VisualizeInvalidMove(move);
                return;
            }
            ApplyStateChange(move);
            await this.gameSessionView.VisualizeMove(move);
        }

        private void ApplyStateChange(MovementMove move)
        {
            var piece = this.boardService.DropPiece(move.From);
            this.boardService.PlacePiece(piece, move.To);
            this.sessionData.CurrentTurn++;
            TryUpdateGameStage();
        }

        private bool IsInvalidMove(MovementMove move)
        {
            return false;
        }

        public Task EndGame()
        {
            return this.gameSessionView.ShowGameOverScreen();
        }

        private void TryUpdateGameStage()
        {
            if (this.boardValidator.HasTickTackToe(boardService)) {
                this.sessionData.State.Stage = Stage.End;
            }
            if (this.sessionData.State.Stage == Stage.Placement) {
                foreach (var player in this.sessionData.Players) {
                    if (player.UnusedPieces.First != null) {
                        return;
                    }
                }

                this.sessionData.State.Stage = Stage.Movement;
            }
        }
    }
}