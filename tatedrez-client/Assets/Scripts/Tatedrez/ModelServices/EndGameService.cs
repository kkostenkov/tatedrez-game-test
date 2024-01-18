using System.Collections.Generic;
using Tatedrez.Models;

namespace Tatedrez.ModelServices
{
    public class EndGameService
    {
        private readonly IBoardInfoService boardInfoService;
        private readonly GameSessionData data;

        public EndGameService(IBoardInfoService boardInfoService, GameSessionData data)
        {
            this.boardInfoService = boardInfoService;
            this.data = data;
        }
        
        public EndGameDetails ComposeEndgameDetails(List<BoardCoords> winningCoords)
        {
            var firstPiece = boardInfoService.PeekPiece(winningCoords[0]);
            var endGameDetails = new EndGameDetails() {
                WinnerId = firstPiece.Owner,
            };
            foreach (var bc in winningCoords) {
                endGameDetails.WinnerCords.Add(bc);
            }

            this.data.EndGameDetails = endGameDetails;
            return endGameDetails;
        }

        public EndGameDetails GetEndGameDetails()
        {
            return this.data.EndGameDetails;
        }
    }
}