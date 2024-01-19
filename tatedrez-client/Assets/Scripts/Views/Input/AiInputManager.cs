using System.Threading.Tasks;
using Bonsai.Core;
using Tatedrez.Interfaces;
using Tatedrez.Models;
using UnityEngine;
using Task = System.Threading.Tasks.Task;

namespace Tatedrez.Input
{
    internal class AiInputManager : MonoBehaviour, IMoveFetcher
    {
        [SerializeField]
        private BehaviourTree behTree;
        
        public Task<PlacementMove> GetMovePiecePlacement()
        {
            var move = new PlacementMove() {
                // PieceGuid = selectedPiece.Guid,
                // To = toCoords
            };
            return Task.FromResult(move);
        }

        public Task<MovementMove> GetMovePieceMovement()
        {
            var move = new MovementMove();
            Debug.Log($"Movemoent move command: {move.PieceGuid}" +
                      $"from {move.From} to {move.To}");
            return Task.FromResult(move);
        }
    }
}