using System.Collections.Generic;
using Tatedrez;
using Tatedrez.Models;
using Tatedrez.ModelServices;

public static class Helpers
{
    public static Board CreateEmptyBoard3by3()
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
            Board = CreateEmptyBoard3by3(),
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
            PieceType = Constants.Knight,
        });
        list.AddLast(new Piece(owner) {
            PieceType = Constants.Rook,
        });
        list.AddLast(new Piece(owner) {
            PieceType = Constants.Bishop,
        });
        return list;
    }

    public static GameSessionDataService CreateDataService()
    {
        var board = new BoardService();
        var gameState = new GameStateService();
        var endGame = new EndGameService(board);
        return new GameSessionDataService(board, gameState, endGame);
    }
    
}