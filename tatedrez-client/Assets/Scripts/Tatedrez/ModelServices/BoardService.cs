using Tatedrez.Models;

namespace Tatedrez.ModelServices
{
    public interface IBoardInfoService
    {
        bool IsOccupied(BoardCoords coords);
        Piece PeekPiece(BoardCoords coords);

        BoardCoords GetSize();
    }

    public class BoardService : IBoardInfoService
    {
        private readonly Board boardData;

        public BoardService(Board boardData)
        {
            this.boardData = boardData;
        }

        public bool IsOccupied(BoardCoords coords)
        {
            var key = ToKey(coords);
            this.boardData.PiecesByCoordinates.TryGetValue(key, out var piece);
            return piece != null;
        }

        public Piece PeekPiece(BoardCoords coords)
        {
            var key = ToKey(coords);
            this.boardData.PiecesByCoordinates.TryGetValue(key, out var piece);
            return piece;
        }

        public BoardCoords GetSize()
        {
            return new BoardCoords(this.boardData.BoardSize.X, this.boardData.BoardSize.Y);
        }

        public void PlacePiece(Piece piece, BoardCoords coords)
        {
            this.boardData.PiecesByCoordinates.Add(ToKey(coords), piece);
        }
        
        public Piece DropPiece(BoardCoords coords)
        {
            var key = ToKey(coords);
            this.boardData.PiecesByCoordinates.TryGetValue(key, out var piece);
            this.boardData.PiecesByCoordinates.Remove(key);
            return piece;
        }

        private int ToKey(BoardCoords coords)
        {
            return coords.X + coords.Y * this.boardData.BoardSize.X;
        }
    }
}