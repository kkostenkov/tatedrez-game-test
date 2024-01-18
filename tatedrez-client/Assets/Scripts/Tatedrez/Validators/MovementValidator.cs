using System;
using System.Collections.Generic;
using Tatedrez.Models;
using Tatedrez.ModelServices;
using Tatedrez.Rules;

namespace Tatedrez.Validators
{
    public class MovementValidator
    {
        private readonly Dictionary<string, IPieceMovesValidator> knownPieceRules = new();

        public MovementValidator()
        {
            AddRule(Constants.Bishop, new BishopRulesHolder());
            AddRule(Constants.Rook, new RookRulesHolder());
            AddRule(Constants.Knight, new KnightRulesHolder());
        }

        public void AddRule(string pieceType, IPieceMovesValidator rules)
        {
            this.knownPieceRules.Add(pieceType, rules);
        }

        public bool IsValidMove(IBoardInfoService board, MovementMove move)
        {
            var movingPiece = board.PeekPiece(move.From);
            if (movingPiece == null || movingPiece.Owner != move.PlayerIndex) {
                return false;
            }
            
            if (board.IsOccupied(move.To)) {
                return false;
            }

            if (!knownPieceRules.TryGetValue(movingPiece.PieceType, out var movingPieceRules)) {
                throw new ArgumentException(movingPiece.PieceType);
            }

            return movingPieceRules.ValidateMove(move, board);
        }
    }
}