using System.Threading.Tasks;
using Bonsai.Core;
using Tatedrez.Interfaces;
using Tatedrez.Models;
using Tatedrez.ModelServices;
using Tatedrez.Views;
using UnityEngine;

namespace Tatedrez.Input
{
    internal class AiInputManager : MonoBehaviour, IInputSource
    {
        [SerializeField]
        private BehaviourTree behTree;
        private IPlayerView playerView;
        private IBoardView boardView;
        private PlayerService player;

        public void Bind(PlayerService player, IPlayerView playerView, IBoardView boardView)
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
            Debug.Log($"Placement move command: {this.playerView.SelectedPiece.PieceType} to {toCoords}");
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
            var move = await GetMove(player.Index);
            Debug.Log($"Movemoent move command: {move.PieceGuid}" +
                      $"from {move.From} to {move.To}");
            return move;
        }
        
        private async Task<MovementMove> GetMove(int playerIndex)
        {
            using MovePartsCollector moveCollector = new MovePartsCollector();
            var task = moveCollector.WaitForMove(playerIndex, this.boardView);
            var result = await task;

            return result;
        }
    }
}