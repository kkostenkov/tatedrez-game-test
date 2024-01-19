using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tatedrez.Interfaces;
using Tatedrez.Models;
using Tatedrez.ModelServices;
using UnityEngine;
using Random = System.Random;

namespace Tatedrez.Input
{
    internal class AiInputManager : MonoBehaviour, IMoveFetcher
    {
        private int playerIndex;
        private IGameSessionDataService sessionDataService;
        private PlayerService playerService;
        private BoardService boardService;
        private IMovesGenerator movesGenerator;
        private readonly Random rand = new Random();

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
            
            var vacantSquares = boardService.GetEmptySquares().ToList();
            var selectedSquare = vacantSquares[this.rand.Next(0, vacantSquares.Count)];
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
            var allMoves = GenerateAllPossibleMoves(myPieces, this.boardService, this.movesGenerator).ToList();
            int index = rand.Next(0, allMoves.Count);
            var move = allMoves[index]; 
            
            Debug.Log($"Movemoent move command: {move.PieceGuid}" +
                      $"from {move.From} to {move.To}");
            return Task.FromResult(move);
        }

        private IEnumerable<MovementMove> GenerateAllPossibleMoves(IEnumerable<Piece> myPieces, BoardService boardService, IMovesGenerator movesGenerator)
        {
            foreach (var piece in myPieces) {
                var position = this.boardService.FindSquares(p => p.Guid == piece.Guid).First();
                var possibleMoves = movesGenerator.GetPossibleMovementDestinations(piece.PieceType, position, boardService);
                foreach (var destination in possibleMoves) {
                    var move = new MovementMove() {
                        PieceGuid = piece.Guid,
                        From = position,
                        To = destination
                    };
                    yield return move;
                }    
            }
        }
    }
}