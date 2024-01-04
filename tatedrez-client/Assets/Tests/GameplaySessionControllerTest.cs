using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using Tatedrez;
using Tatedrez.Models;

public class GameplaySessionControllerTest
{
    [TestCase(0, 0, ExpectedResult = 1)]
    [TestCase(10, 0, ExpectedResult = 11)]
    [TestCase(0, 1, ExpectedResult = 1)]
    [TestCase(4, 1, ExpectedResult = 5)]
    public async Task<int> Should_IncrementTurnNumber_When_PlayerPutsPiece(int startTurnNumber, int movingPlayerIndex)
    {
        var sessionData = new GameSessionData {
            CurrentPlayerTurnIndex = startTurnNumber,
            Players = new List<Player>() { new Player(), new Player() },
            State = new GameState() {Stage = Stage.Placement },
        };
        var view = Substitute.For<IBoardView>();
        var input = Substitute.For<IInputManger>();
        var controller = new GameSessionController(sessionData, view, input);

        await controller.Turn();
        return await Task.FromResult(sessionData.CurrentPlayerTurnIndex);
    }

    [TestCase(0, 0, ExpectedResult = 1)]
    [TestCase(10, 0, ExpectedResult = 11)]
    [TestCase(0, 1, ExpectedResult = 1)]
    [TestCase(4, 1, ExpectedResult = 5)]
    public async Task<int> Should_IncrementTurnNumber_When_PlayerMovesPiece(int startTurnNumber, int movingPlayerIndex)
    {
        var sessionData = new GameSessionData {
            CurrentPlayerTurnIndex = startTurnNumber,
            Players = new List<Player>() { new Player(), new Player() },
            State = new GameState() { Stage = Stage.Placement },
        };
        var view = Substitute.For<IBoardView>();
        var input = Substitute.For<IInputManger>();
        var controller = new GameSessionController(sessionData, view, input);

        await controller.Turn();
        return await Task.FromResult(sessionData.CurrentPlayerTurnIndex);
    }

    [Test]
    public async Task Should_PlacePieceOnBoard_When_PlayerMakesPlaceMove()
    {
        var sessionData = CreateStandardSessionStart();
        var pieceToPlace = sessionData.Players[0].UnusedPieces.First.Value;
        var view = Substitute.For<IBoardView>();
        var input = Substitute.For<IInputManger>();
        var placementCoords = new BoardCoords { X = 1, Y = 1 };
        input.GetMovePiecePlacement(0).Returns(new PlacementMove() {
            PieceGuid = pieceToPlace.Guid,
            PlayerIndex = 0,
            To = placementCoords,
        });
        var controller = new GameSessionController(sessionData, view, input);
        
        await controller.Turn();

        var placedPiece = sessionData.Board.PeekPiece(placementCoords);
        Assert.IsNotNull(placedPiece);
        Assert.AreEqual(pieceToPlace.Guid, placedPiece.Guid);
    }

    [Test]
    public async Task Should_RemovePieceFromPlayer_When_PlayerMakesPlaceMove()
    {
        var sessionData = CreateStandardSessionStart();
        var placingPlayer = sessionData.Players[0]; 
        var pieceToPlace = placingPlayer.UnusedPieces.First.Value;
        var placedPieceGuid = pieceToPlace.Guid; 
        var view = Substitute.For<IBoardView>();
        var input = Substitute.For<IInputManger>();
        var placementCoords = new BoardCoords { X = 1, Y = 1 };
        input.GetMovePiecePlacement(0).Returns(new PlacementMove() {
            PieceGuid = pieceToPlace.Guid,
            PlayerIndex = 0,
            To = placementCoords,
        });
        var controller = new GameSessionController(sessionData, view, input);
        
        await controller.Turn();

        var droppedPiece = placingPlayer.UnusedPieces.FirstOrDefault(p => p.Guid == placedPieceGuid);
        Assert.IsNull(droppedPiece);
    }
    
    [Test]
    public async Task Should_ChangeStageFromPlacementToMovement_When_PlayersHaveNoPieces()
    {
        var view = Substitute.For<IBoardView>();
        var input = Substitute.For<IInputManger>();
        var sessionData = new GameSessionData() {
            Board = CreateDefaultBoard(),
            State = new GameState() { Stage = Stage.Placement },
        };
        // 0
        var placingPlayer = CreatePlayerWithOnePiece(); 
        sessionData.Players.Add(placingPlayer);
        var pieceToPlaceGuid = placingPlayer.UnusedPieces.First.Value.Guid;
        var placementCoords = new BoardCoords { X = 1, Y = 1 };
        input.GetMovePiecePlacement(0).Returns(new PlacementMove() {
            PieceGuid = pieceToPlaceGuid,
            PlayerIndex = 0,
            To = placementCoords,
        });
        // 1
        placingPlayer = CreatePlayerWithOnePiece(); 
        sessionData.Players.Add(placingPlayer);
        pieceToPlaceGuid = placingPlayer.UnusedPieces.First.Value.Guid;
        placementCoords = new BoardCoords { X = 2, Y = 2 };
        input.GetMovePiecePlacement(1).Returns(new PlacementMove() {
            PieceGuid = pieceToPlaceGuid,
            PlayerIndex = 1,
            To = placementCoords,
        });
        var controller = new GameSessionController(sessionData, view, input);
        
        await controller.Turn();
        await controller.Turn();

        var currentStage = sessionData.State.Stage;
        Assert.AreEqual(Stage.Movement, currentStage);
    }

    private GameSessionData CreateStandardSessionStart()
    {
        return new GameSessionData() {
            Board = CreateDefaultBoard(),
            CurrentPlayerTurnIndex = 0,
            Players = new List<Player>() {
                new Player() {
                    UnusedPieces = CreateStartPieces(),
                },
                new Player() {
                    UnusedPieces = CreateStartPieces(),
                }
            },
            State = new GameState() {
                Stage = Stage.Placement
            }
        };
    }

    private Board CreateDefaultBoard()
    {
        return new Board() {
            BoardSize = new BoardCoords() {
                X = 3,
                Y = 3,
            },
            PiecesByCoordinates = new(),
        };
    }

    private Player CreatePlayerWithOnePiece()
    {
        var player = new Player();
        player.UnusedPieces.AddFirst(new Piece());
        return player;
    }

    public LinkedList<Piece> CreateStartPieces()
    {
        var list = new LinkedList<Piece>();
        list.AddLast(new Piece() {
            PieceType = "Knight",
        });
        list.AddLast(new Piece() {
            PieceType = "Rook",
        });
        list.AddLast(new Piece() {
            PieceType = "Bishop",
        });
        return list;
    }
}