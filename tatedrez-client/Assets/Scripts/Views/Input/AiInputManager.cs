using System.Threading.Tasks;
using Bonsai.Core;
using Tatedrez.Interfaces;
using Tatedrez.Models;
using UnityEngine;

namespace Tatedrez.Input
{
    internal class AiInputManager : MonoBehaviour, IInputSource
    {
        [SerializeField]
        private BehaviourTree behTree;
        
        public async Task<PlacementMove> GetMovePiecePlacement()
        {
            var move = new PlacementMove() {
                // PieceGuid = selectedPiece.Guid,
                // To = toCoords
            };
            return move;
        }

        public async Task<MovementMove> GetMovePieceMovement()
        {
            var move = new MovementMove();
            Debug.Log($"Movemoent move command: {move.PieceGuid}" +
                      $"from {move.From} to {move.To}");
            return move;
        }
    }
}