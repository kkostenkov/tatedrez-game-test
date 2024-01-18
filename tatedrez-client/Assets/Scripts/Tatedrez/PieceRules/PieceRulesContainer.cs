using System;
using System.Collections.Generic;
using Tatedrez.Validators;

namespace Tatedrez.Rules
{
    public class PieceRulesContainer
    {
        private readonly Dictionary<string, IPieceRulesHolder> knownPieceRules = new();

        public PieceRulesContainer()
        {
            AddRule(Constants.Bishop, new BishopRulesHolder());
            AddRule(Constants.Rook, new RookRulesHolder());
            AddRule(Constants.Knight, new KnightRulesHolder());
        }

        public void AddRule(string pieceType, IPieceRulesHolder generator)
        {
            this.knownPieceRules.Add(pieceType, generator);
        }

        public IPieceRulesHolder GetPieceRules(string pieceType)
        {
            if (!this.knownPieceRules.TryGetValue(pieceType, out var pieceMovesGenerator)) {
                throw new ArgumentException(pieceType);
            }

            return pieceMovesGenerator;
        }
    }
}