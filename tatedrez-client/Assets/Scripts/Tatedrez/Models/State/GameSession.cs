using System.Collections.Generic;

namespace Tatedrez.Models
{
    public class GameSession
    {
        public Board Board = new();
        public GameState State = new();
        public List<Player> Players = new(); // color is index % 2
        public int CurrentPlayerTurnIndex;
    }
}