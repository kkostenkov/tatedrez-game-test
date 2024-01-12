using System;
using System.Collections.Generic;
using System.Linq;
using Tatedrez.Models;

namespace Tatedrez.ModelServices
{
    public class PlayerService
    {
        private readonly Player playerData;
        private readonly int playerIndex;

        public int Index => this.playerIndex;

        public PlayerService(Player playerData, int playerIndex)
        {
            this.playerData = playerData;
            this.playerIndex = playerIndex;
        }

        public string GetName()
        {
            return $"Player {this.playerIndex + 1}";
        }

        public IEnumerable<Piece> Pieces()
        {
            return this.playerData.UnusedPieces;
        }
        
        public Piece DropPiece(Guid guidToDrop)
        {
            var pieceToDrop = playerData.UnusedPieces.FirstOrDefault(p => p.Guid == guidToDrop);
            if (pieceToDrop == null) {
                return null;
            }

            playerData.UnusedPieces.Remove(pieceToDrop);
            return pieceToDrop;
        }
    }
}