using System;
using System.Collections.Generic;
using Tatedrez.Models;
using Tatedrez.ModelServices;

namespace Tatedrez.Rules
{
    internal class MovesGenerator
    {
        private readonly Dictionary<string, IPieceMovesGenerator> knownPieceRules = new();

        public MovesGenerator()
        {
            AddRule(Constants.Bishop, new BishopRulesHolder());
            AddRule(Constants.Rook, new RookRulesHolder());
            AddRule(Constants.Knight, new KnightRulesHolder());
        }

        public void AddRule(string pieceType, IPieceMovesGenerator generator)
        {
            this.knownPieceRules.Add(pieceType, generator);
        }
        
        public bool HasMoves(Piece piece, BoardCoords position, IBoardInfoService board)
        {
            if (!this.knownPieceRules.TryGetValue(piece.PieceType, out var pieceMovesGenerator)) {
                throw new ArgumentException(piece.PieceType);
            }

            return pieceMovesGenerator.HasLegitMoves(position, board);
        }
    }
}