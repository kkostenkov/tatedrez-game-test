using System.Collections.Generic;

namespace Tatedrez.Models
{
    public class GameSessionData
    {
        public Board Board = new();
        public GameState State = new();
        public List<Player> Players = new(); // color is index % 2
        public int CurrentTurn;
        public EndGameDetails EndGameDetails = new();
    }
}