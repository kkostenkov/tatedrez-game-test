using System.Collections.Generic;
using System.Threading.Tasks;
using Tatedrez.Interfaces;
using Tatedrez.Models;

namespace Tatedrez.Input
{
    public class PlayerInputManager : IInputManager
    {
        private int playerIndexToListenForMoves;
        private Dictionary<int, IInputSource> playerInputs = new();

        public void AddInputSource(IInputSource input, int playerIndex)
        {
            this.playerInputs.Add(playerIndex, input);
        }

        public IInputSource GetInputSource(int playerIndex)
        {
            return this.playerInputs[playerIndex];
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