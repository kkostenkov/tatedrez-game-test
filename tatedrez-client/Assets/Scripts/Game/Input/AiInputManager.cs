using System.Threading.Tasks;
using Tatedrez.AI;
using Tatedrez.Interfaces;
using Tatedrez.Models;
using Tatedrez.ModelServices;
using UnityEngine;

namespace Tatedrez.Input
{
    public class AiInputManager : MonoBehaviour, IMoveFetcher
    {
        private int playerIndex;
        private IGameSessionDataService sessionDataService;
        private PlayerService playerService;
        private BoardService boardService;
        private IMovesGenerator movesGenerator;
        private readonly IAiMovesGenerator aiRandomBrain = new AiRandomBrain();

        public void Initialize(int playerIndex)
        {
            this.playerIndex = playerIndex;
            sessionDataService = DI.Game.Resolve<IGameSessionDataService>();
            this.playerService = this.sessionDataService.GetPlayer(this.playerIndex);
            this.playerService.SetName("bot");
            this.boardService = this.sessionDataService.BoardService;
            this.movesGenerator = DI.Game.Resolve<IMovesGenerator>();
        }

        public Task<PlacementMove> GetMovePiecePlacement()
        {
            var move = this.aiRandomBrain.GeneratePlacementMove(this.playerIndex, this.playerService, this.boardService);
            return move;
        }

        public Task<MovementMove> GetMovePieceMovement()
        {
            var move = this.aiRandomBrain.GenerateMovementMove(this.playerIndex, this.boardService, this.movesGenerator);

            Debug.Log($"Movement move command: {move.PieceGuid}" +
                      $"from {move.From} to {move.To}");
            return Task.FromResult(move);
        }
    }
}