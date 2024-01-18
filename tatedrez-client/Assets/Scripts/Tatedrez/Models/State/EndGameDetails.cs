using System.Collections.Generic;

namespace Tatedrez.Models
{
    public class EndGameDetails
    {
        public int WinnerId = -1;
        public readonly List<BoardCoords> WinnerCords = new List<BoardCoords>();
    }
}