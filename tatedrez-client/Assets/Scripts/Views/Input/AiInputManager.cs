using System.Linq;
using System.Threading.Tasks;
using Bonsai.Core;
using Tatedrez.Interfaces;
using Tatedrez.Models;
using Tatedrez.ModelServices;
using UnityEngine;
using Task = System.Threading.Tasks.Task;

namespace Tatedrez.Input
{
    internal class AiInputManager : MonoBehaviour, IMoveFetcher
    {
        [SerializeField]
        private BehaviourTree behTree;

        private int playerIndex;

        public void SetPlayerIndex(int playerIndex)
        {
            this.playerIndex = playerIndex;
        }

        public Task<PlacementMove> GetMovePiecePlacement()
        {
            var sessionDataService = DI.Game.Resolve<IGameSessionDataService>();
            var playerService = sessionDataService.GetPlayer(this.playerIndex);
            var unusedPieces = playerService.Pieces();
            var selectedPiece = unusedPieces.First();

            var boardService = sessionDataService.BoardService;
            var vacantSquares = boardService.GetEmptySquares();
            var selectedSquare = vacantSquares.First();
            var move = new PlacementMove() {
                PieceGuid = selectedPiece.Guid,
                To = selectedSquare
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