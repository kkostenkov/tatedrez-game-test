using System.Threading.Tasks;
using Tatedrez.Interfaces;
using Tatedrez.Models;
using UnityEngine;

namespace Tatedrez.Input
{
    public class LocalInputManager : MonoBehaviour, IMoveFetcher
    {
        private IPlayerView playerView;
        private IBoardView boardView;
        private int playerIndex;

        public void SetViews(IPlayerView playerView, IBoardView boardView, int playerIndex)
        {
            this.playerView = playerView;
            this.boardView = boardView;
            this.playerIndex = playerIndex;
        }
        
        public async Task<PlacementMove> GetMovePiecePlacement()
        {
            playerView.EnablePieceSelection();
            BoardCoords toCoords = BoardCoords.Invalid;
            while (this.playerView.SelectedPiece == null) {
                toCoords = await boardView.GetSelectedSquareAsync();
            }

            var selectedPiece = this.playerView.SelectedPiece;
            Debug.Log($"Placement move command: {this.playerView.SelectedPiece.PieceType} to {toCoords}");
            playerView.DisablePieceSelection();

            var move = new PlacementMove() {
                PlayerIndex = this.playerIndex,
                PieceGuid = selectedPiece.Guid,
                To = toCoords
            };
            return move;
        }

        public async Task<MovementMove> GetMovePieceMovement()
        {
            var move = await GetMove();
            Debug.Log($"Movemoent move command: {move.PieceGuid}" +
                      $"from {move.From} to {move.To}");
            return move;
        }

        private async Task<MovementMove> GetMove()
        {
            using MovePartsCollector moveCollector = new MovePartsCollector();
            var task = moveCollector.WaitForMove(this.playerIndex, this.boardView);
            var result = await task;

            return result;
        }
    }
}