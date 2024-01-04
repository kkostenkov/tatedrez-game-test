using System;

namespace Tatedrez.Models
{
    public class Move
    {
        public int PlayerIndex;
    }
    
    public class PieceMove : Move //TODO: split into files
    {
        public Guid PieceGuid;
    }

    public class PlacementMove : PieceMove
    {
        public BoardCoords To;
    }
    
    public class MovementMove : PieceMove
    {
        public BoardCoords From;
        public BoardCoords To;
    }
}