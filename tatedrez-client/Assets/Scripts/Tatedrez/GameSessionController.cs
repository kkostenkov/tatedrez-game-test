using System;
using System.Threading.Tasks;
using Tatedrez.Models;
using Tatedrez.ModelServices;

namespace Tatedrez
{
    public class GameSessionController
    {
        private readonly GameSessionData sessionData;
        private readonly IGameSessionView gameSessionView;
        private readonly IInputManger input;
        private readonly BoardValidator boardValidator;
        
        private readonly GameSessionDataService sessionDataService;
        private readonly BoardService boardService;

        public GameSessionController(GameSessionData sessionData, IGameSessionView gameSessionView, IInputManger input)
        {
            this.sessionData = sessionData;
            this.sessionDataService = new GameSessionDataService(sessionData);
            this.boardService = this.sessionDataService.BoardService;
            this.gameSessionView = gameSessionView;
            this.input = input;
            this.boardValidator = new BoardValidator();
        }

        public Task Turn()
        {
            var playerTurnIndex = this.sessionData.CurrentPlayerTurnIndex % this.sessionData.Players.Count;
            var state = this.sessionData.State;
            return state.Stage switch {
                Stage.Unknown => Task.CompletedTask,
                Stage.Placement => PlacePieceByPlayer(playerTurnIndex),
                Stage.Movement => MovePieceByPlayer(playerTurnIndex),
                Stage.End => EndGame(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public Task BuildBoardAsync()
        {
            return this.gameSessionView.Build(this.sessionDataService);
        }

        private async Task PlacePieceByPlayer(int playerIndex)
        {
            await this.gameSessionView.ShowTurn(playerIndex);
            var move = await input.GetMovePiecePlacement(playerIndex);
            
            if (await TryProcessInvalidPacementMove(move)) {
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
            this.sessionData.CurrentPlayerTurnIndex++;
            TryUpdateGameStage();
        }

        private async Task<bool> TryProcessInvalidPacementMove(PlacementMove move)
        {
            if (this.boardValidator.IsValidMove(this.boardService, move)) {
                return false;
            }
            await this.gameSessionView.VisualizeInvalidMove(move);
            return true;
        }

        private async Task MovePieceByPlayer(int playerIndex)  
        {
            await this.gameSessionView.ShowTurn(playerIndex);
            // validate if there are available moves
            
            var move = await input.GetMovePieceMovement(playerIndex); // via input manager?
            // validate move via validator
            
            // check and apply state change
            this.sessionData.CurrentPlayerTurnIndex++;
            // update pieces view via board view
            await this.gameSessionView.VisualizeMove(move);
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
                return;
            }
        }
    }
}