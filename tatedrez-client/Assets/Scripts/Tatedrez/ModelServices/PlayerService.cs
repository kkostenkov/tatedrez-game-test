using System.Collections.Generic;
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
            return $"Player {this.playerIndex}";
        }

        public IEnumerable<Piece> Pieces()
        {
            return this.playerData.UnusedPieces;
        }
    }
}