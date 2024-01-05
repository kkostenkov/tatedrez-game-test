using System.Collections.Generic;
using Tatedrez.Models;

namespace Tatedrez
{
    public class EndGameDetails
    {
        public int WinnerId = -1;
        public readonly List<BoardCoords> WinnerCords = new List<BoardCoords>();

        public bool HasWinner => this.WinnerId != -1;
        private const int MinSequenceToWinLength = 3;
        
        public EndGameDetails()
        {
            for (int i = 0; i < MinSequenceToWinLength; i++) {
                WinnerCords.Add(BoardCoords.Invalid);  
            }
        }
            
        public void Clear()
        {
            this.WinnerId = -1;
            for (var index = 0; index < 3; index++) {
                this.WinnerCords[index] = BoardCoords.Invalid;
            }
        }
    }
}