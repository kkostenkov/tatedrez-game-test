using System;
using System.Threading.Tasks;
using Tatedrez.Models;

namespace Tatedrez
{
    public class GameSessionController
    {
        private readonly GameSessionData sessionData;
        private readonly IBoardView boardView;
        private readonly IInputManger input;

        public GameSessionController(GameSessionData sessionData, IBoardView boardView, IInputManger input)
        {
            this.sessionData = sessionData;
            this.boardView = boardView;
            this.input = input;
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
            return boardView.Build(this.sessionData);
        }

        private async Task PlacePieceByPlayer(int playerIndex)
        {
            await boardView.ShowTurn(playerIndex);
            var move = await input.GetMovePiecePlacement(playerIndex); // via input manager?
            // validate move via validator
            
            // check and apply state change
            var player = this.sessionData.Players[playerIndex];
            var piece = player.DropPiece(move.PieceGuid);
            this.sessionData.Board.PlacePiece(piece, move.To);
            this.sessionData.CurrentPlayerTurnIndex++;
            TryUpdateGameStage();
            // update pieces view via board view (relay the move?)
            await boardView.VisualizeMove(move);
        }

        private async Task MovePieceByPlayer(int playerIndex)  
        {
            await boardView.ShowTurn(playerIndex);
            // validate if there are available moves
            
            var move = await input.GetMovePieceMovement(playerIndex); // via input manager?
            // validate move via validator
            
            // check and apply state change
            this.sessionData.CurrentPlayerTurnIndex++;
            // update pieces view via board view
            await boardView.VisualizeMove(move);
        }

        public Task EndGame()
        {
            return boardView.ShowGameOverScreen();
        }

        private void TryUpdateGameStage()
        {
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