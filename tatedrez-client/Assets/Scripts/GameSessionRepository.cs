using System.Collections.Generic;
using Tatedrez.Models;

namespace Tatedrez
{
    internal class GameSessionRepository
    {
        public GameSessionData Load()
        {
            return CreateStandardSessionStart();
        }
   
        private static GameSessionData CreateStandardSessionStart()
        {
            return new GameSessionData() {
                Board = new Board() {
                    BoardSize = new BoardCoords() {
                        X = 3,
                        Y = 3,
                    },
                    PiecesByCoordinates = new(),
                },
                CurrentTurn = 0,
                Players = new List<Player>() {
                    new Player() {
                        UnusedPieces = CreateStartPiecesForPlayer(0),
                    },
                    new Player() {
                        UnusedPieces = CreateStartPiecesForPlayer(1),
                    }
                },
                State = new GameState() {
                    Stage = Stage.Placement
                }
            };
        }
   
        private static LinkedList<Piece> CreateStartPiecesForPlayer(int owner)
        {
            var list = new LinkedList<Piece>();
            list.AddLast(new Piece(owner) {
                PieceType = "Knight",
            });
            list.AddLast(new Piece(owner) {
                PieceType = "Rook",
            });
            list.AddLast(new Piece(owner) {
                PieceType = "Bishop",
            });
            return list;
        }
    }
}