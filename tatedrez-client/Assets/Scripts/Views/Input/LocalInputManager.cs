using System.Threading.Tasks;
using Tatedrez.Models;
using Tatedrez.ModelServices;
using Tatedrez.Views;
using UnityEngine;

namespace Tatedrez.Input
{
    internal class LocalInputManager : MonoBehaviour, IMoveFetcher
    {
        private PlayerView playerView;
        private BoardView boardView;
        private PlayerService player;

        public void Bind(PlayerService player, PlayerView playerView, BoardView boardView)
        {
            this.player = player;
            this.playerView = playerView;
            this.boardView = boardView;
        }

        public async Task<PlacementMove> GetMovePiecePlacement()
        {
            playerView.EnablePieceSelection();
            BoardCoords toCoords = BoardCoords.Invalid;
            while (this.playerView.SelectedPiece == null) {
                toCoords = await boardView.GetSelectedSquareAsync();
            }

            var selectedPiece = this.playerView.SelectedPiece;
            Debug.Log($"Placement move done: {this.playerView.SelectedPiece.PieceType} to {toCoords}");
            playerView.DisablePieceSelection();

            var move = new PlacementMove() {
                PieceGuid = selectedPiece.Guid,
                PlayerIndex = player.Index,
                To = toCoords
            };
            return move;
        }

        public async Task<MovementMove> GetMovePieceMovement()
        {
            var move = await boardView.GetMove(player.Index);
            Debug.Log($"Movemoent move done: {move.PieceGuid}" +
                      $"from {move.From} to {move.To}");
            return move;
        }
    }
}