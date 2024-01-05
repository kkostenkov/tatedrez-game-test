namespace Tatedrez.Models
{
    public struct BoardCoords
    {
        public static BoardCoords Invalid = new BoardCoords(-1, -1);
        
        public int X;
        public int Y;

        public BoardCoords(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public override string ToString()
        {
            return $"{X}:{Y}";
        }
    }
}