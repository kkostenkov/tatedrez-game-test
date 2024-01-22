using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tatedrez.Models;
using Tatedrez.ModelServices;

namespace Tatedrez.AI
{
    public class AiRandomBrain : IAiMovesGenerator
    {
        private readonly Random rand = new Random();

        public MovementMove GenerateMovementMove(int playerIndex, IBoardInfoService boardService, IMovesGenerator movesGenerator)
        {
            var myPieces = boardService.FindPieces(p => p.Owner == playerIndex);
            var allMoves = GenerateAllPossibleMoves(myPieces, boardService, movesGenerator).ToList<MovementMove>();
            int index = this.rand.Next(0, allMoves.Count);
            var move = allMoves[index];
            return move;
        }

        private IEnumerable<MovementMove> GenerateAllPossibleMoves(IEnumerable<Piece> myPieces, IBoardInfoService boardService, IMovesGenerator movesGenerator)
        {
            foreach (var piece in myPieces) {
                var position = boardService.FindSquares(p => p.Guid == piece.Guid).First();
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

        public Task<PlacementMove> GeneratePlacementMove(int playerIndex, PlayerService playerService, IBoardInfoService service)
        {
            var unusedPieces = playerService.Pieces().ToList();
            var selectedPiece = unusedPieces[this.rand.Next(0, unusedPieces.Count)];
            
            var vacantSquares = service.GetEmptySquares().ToList();
            var selectedSquare = vacantSquares[this.rand.Next(0, vacantSquares.Count)];
            var move = new PlacementMove() {
                PlayerIndex = playerIndex,
                PieceGuid = selectedPiece.Guid,
                To = selectedSquare
            };
            return Task.FromResult(move);
        }
    }
}