using System.Linq;
using System.Threading.Tasks;
using Tatedrez.Interfaces;
using Tatedrez.Models;
using Tatedrez.ModelServices;
using UnityEngine;

namespace Tatedrez.Input
{
    internal class AiInputManager : MonoBehaviour, IMoveFetcher
    {
        private int playerIndex;
        private IGameSessionDataService sessionDataService;
        private PlayerService playerService;
        private BoardService boardService;
        private IMovesGenerator movesGenerator;

        public void SetPlayerIndex(int playerIndex)
        {
            this.playerIndex = playerIndex;
            sessionDataService = DI.Game.Resolve<IGameSessionDataService>();
            this.playerService = this.sessionDataService.GetPlayer(this.playerIndex);
            this.boardService = this.sessionDataService.BoardService;
            this.movesGenerator = DI.Game.Resolve<IMovesGenerator>();
        }

        public Task<PlacementMove> GetMovePiecePlacement()
        {
            var unusedPieces = playerService.Pieces();
            var selectedPiece = unusedPieces.First();
            
            var vacantSquares = boardService.GetEmptySquares();
            var selectedSquare = vacantSquares.First();
            var move = new PlacementMove() {
                PlayerIndex = this.playerIndex,
                PieceGuid = selectedPiece.Guid,
                To = selectedSquare
            };
            return Task.FromResult(move);
        }

        public Task<MovementMove> GetMovePieceMovement()
        {
            var myPieces = this.boardService.FindPieces(p => p.Owner == this.playerIndex);
            var selected = myPieces.First();
            var position = this.boardService.FindSquares(p => p.Guid == selected.Guid).First();
            var possibleMoves = movesGenerator.GetPossibleMovementDestinations(selected.PieceType, position, this.boardService);
            var destination = possibleMoves.First();
            
            var move = new MovementMove() {
                PieceGuid = selected.Guid,
                PlayerIndex = this.playerIndex,
                From = position,
                To = destination
            };   
            Debug.Log($"Movemoent move command: {move.PieceGuid}" +
                      $"from {move.From} to {move.To}");
            return Task.FromResult(move);
        }
    }
}