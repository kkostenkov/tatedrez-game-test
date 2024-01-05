using System.Collections.Generic;
using Tatedrez.Models;

namespace Tatedrez.Tests.Helpers
{
    public static class Helpers
    {
        public static Board CreateEmptyBoard()
        {
            return new Board() {
                BoardSize = new BoardCoords() {
                    X = 3,
                    Y = 3,
                },
                PiecesByCoordinates = new(),
            };
        }

        public static GameSessionData CreateStandardSessionStart()
        {
            return new GameSessionData() {
                Board = CreateEmptyBoard(),
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

        public static Player CreatePlayerWithOnePiece(int playerIndex)
        {
            var player = new Player();
            player.UnusedPieces.AddFirst(new Piece(playerIndex));
            return player;
        }

        public static LinkedList<Piece> CreateStartPiecesForPlayer(int owner)
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