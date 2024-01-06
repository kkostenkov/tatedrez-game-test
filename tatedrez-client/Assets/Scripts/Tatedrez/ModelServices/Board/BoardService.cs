using System;
using System.Collections.Generic;
using System.Linq;
using Tatedrez.Models;
using Tatedrez.Rules;

namespace Tatedrez.ModelServices
{
    public class BoardService : IBoardInfoService, IBoardModifier
    {
        private readonly Board boardData;
        private readonly MovesGenerator movesGenerator;

        public BoardService(Board boardData)
        {
            this.boardData = boardData;
            this.movesGenerator = new MovesGenerator();
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

        private BoardCoords ToCoords(int key)
        {
            var x = key % this.boardData.BoardSize.X;
            var y = key / this.boardData.BoardSize.X;
            return new BoardCoords(x, y);
        }

        public IEnumerable<Piece> FindPieces(Func<Piece, bool> checkerFunc)
        {
            var foundPieces = boardData.PiecesByCoordinates.Values.Where(checkerFunc);
            return foundPieces;
        }

        public IEnumerable<BoardCoords> FindSquares(Func<Piece, bool> checkerFunc)
        {
            var foundSquares = boardData.PiecesByCoordinates
                .Where(kvp => checkerFunc(kvp.Value))
                .Select(kvp => ToCoords(kvp.Key));
            return foundSquares;
        }

        public bool PlayerHasMoves(int activePlayerIndex)
        {
            var playerSquares = FindSquares(p => p.Owner == activePlayerIndex);
            foreach (var cords in playerSquares) {
                var piece = PeekPiece(cords);
                var hasMoves = this.movesGenerator.HasMoves(piece, cords, this);
                if (hasMoves) {
                    return true;
                }
            }

            return false;
        }
    }
}