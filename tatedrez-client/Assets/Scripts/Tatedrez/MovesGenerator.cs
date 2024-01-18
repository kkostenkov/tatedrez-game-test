using System.Collections.Generic;
using Tatedrez.Models;
using Tatedrez.ModelServices;
using Tatedrez.Rules;

namespace Tatedrez
{
    public class MovesGenerator
    {
        private readonly IBoardInfoService boardService;
        private readonly PieceRulesContainer pieceRules;

        public MovesGenerator(IBoardInfoService boardService, PieceRulesContainer pieceRules)
        {
            this.boardService = boardService;
            this.pieceRules = pieceRules;
        }

        public bool PlayerHasMoves(int activePlayerIndex)
        {
            var playerSquares = this.boardService.FindSquares(p => p.Owner == activePlayerIndex);
            foreach (var cords in playerSquares) {
                var piece = this.boardService.PeekPiece(cords);
                var hasMoves = this.HasMoves(piece, cords, this.boardService);
                if (hasMoves) {
                    return true;
                }
            }

            return false;
        }
        
        public bool HasMoves(Piece piece, BoardCoords position, IBoardInfoService board)
        {
            return this.pieceRules.GetPieceRules(piece.PieceType).HasLegitMoves(position, board);
        }
        
        public IEnumerable<BoardCoords> GetLegitMovementDestinations(string pieceType, BoardCoords position, IBoardInfoService board)
        {
            return this.pieceRules.GetPieceRules(pieceType).GetLegitMovementDestinations(position, board);
        }
    }
}