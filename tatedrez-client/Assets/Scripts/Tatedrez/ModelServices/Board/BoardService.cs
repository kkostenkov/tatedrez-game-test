using System;
using System.Collections.Generic;
using System.Linq;
using Tatedrez.Models;

namespace Tatedrez.ModelServices
{
    public class BoardService : IBoardInfoService, IBoardModifier
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

        public bool HasSquare(BoardCoords coords)
        {
            var size = this.boardData.BoardSize;
            return coords.X >= 0 && coords.X < size.X &&
                   coords.Y >= 0 && coords.Y < size.Y;
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

        public IEnumerable<Piece> FindPieces(Func<Piece, bool> checkerFunc)
        {
            var foundPieces = boardData.PiecesByCoordinates.Values.Where(checkerFunc);
            return foundPieces;
        }
    }
}