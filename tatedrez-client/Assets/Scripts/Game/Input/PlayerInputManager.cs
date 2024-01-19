using System.Collections.Generic;
using System.Threading.Tasks;
using Tatedrez.Interfaces;
using Tatedrez.Models;

namespace Tatedrez.Input
{
    public class PlayerInputManager : IInputManager
    {
        private int playerIndexToListenForMoves;
        private Dictionary<int, IMoveFetcher> playerInputs = new();

        public void AddInputSource(IMoveFetcher input, int playerIndex)
        {
            this.playerInputs.Add(playerIndex, input);
        }

        public IMoveFetcher GetInputSource(int playerIndex)
        {
            return this.playerInputs[playerIndex];
        }

        public void SetActivePlayer(int playerIndex)
        {
            this.playerIndexToListenForMoves = playerIndex;
        }
   
        public async Task<PlacementMove> GetMovePiecePlacement()
        {
            var move = await GetCurrrentInputSource().GetMovePiecePlacement();
            move.PlayerIndex = this.playerIndexToListenForMoves;
            return move;
        }

        public async Task<MovementMove> GetMovePieceMovement()
        {
            var playerIndex = this.playerIndexToListenForMoves;
            var move = await GetCurrrentInputSource().GetMovePieceMovement();
            move.PlayerIndex = playerIndex;
            return move;
        }

        private IMoveFetcher GetCurrrentInputSource()
        {
            return this.playerInputs[this.playerIndexToListenForMoves];
        }
    }
}