using System.Threading.Tasks;
using Tatedrez.Models;
using Tatedrez.ModelServices;
using UnityEngine;

namespace Tatedrez.Views
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
            Debug.Log($"Placement move done: {this.playerView.SelectedPiece.PieceType} to {toCoords.ToString()}");
            playerView.DisablePieceSelection();

            PlacementMove move = new PlacementMove() {
                PieceGuid = selectedPiece.Guid,
                PlayerIndex = player.Index,
                To = toCoords
            };
            return move;
        }

        public Task<MovementMove> GetMovePieceMovement()
        {
            throw new System.NotImplementedException();
        }
    }
}