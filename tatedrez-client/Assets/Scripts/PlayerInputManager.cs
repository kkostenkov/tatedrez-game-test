using System.Collections.Generic;
using System.Threading.Tasks;
using Tatedrez.Models;

namespace Tatedrez
{
    internal class PlayerInputManager : IMoveFetcher, IActivePlayerIndexListener
    {
        private int playerIndexToListenForMoves;
        private Dictionary<int, IMoveFetcher> playerInputs = new();

        public void AddInputSource(IMoveFetcher input, int playerIndex)
        {
            this.playerInputs.Add(playerIndex, input);
        }

        public void SetActivePlayer(int playerIndex)
        {
            this.playerIndexToListenForMoves = playerIndex;
        }
   
        public Task<PlacementMove> GetMovePiecePlacement()
        {
            return GetCurrrentInputSource().GetMovePiecePlacement();
        }

        public Task<MovementMove> GetMovePieceMovement()
        {
            return GetCurrrentInputSource().GetMovePieceMovement();
        }

        private IMoveFetcher GetCurrrentInputSource()
        {
            return this.playerInputs[this.playerIndexToListenForMoves];
        }
   
    }
}