using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using Tatedrez;
using Tatedrez.Models;
using Tatedrez.ModelServices;
using Tatedrez.Tests.Helpers;

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
        var view = Substitute.For<IGameSessionView>();
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
        var view = Substitute.For<IGameSessionView>();
        var input = Substitute.For<IInputManger>();
        var controller = new GameSessionController(sessionData, view, input);

        await controller.Turn();
        return await Task.FromResult(sessionData.CurrentPlayerTurnIndex);
    }
    
    [Test]
    public async Task Should_KeepTurnNumber_When_PlayerMakesInvalidPlacementMove()
    {
        var sessionData = Helpers.CreateStandardSessionStart();
        var occupiedCoords = new BoardCoords(1, 2);
        var boardService = new BoardService(sessionData.Board);
        boardService.PlacePiece(new Piece(0), occupiedCoords);
        var view = Substitute.For<IGameSessionView>();
        var input = Substitute.For<IInputManger>();
        var pieceGuidToPlace = sessionData.Players[0].UnusedPieces.First.Value.Guid;
        input.GetMovePiecePlacement(0).Returns(new PlacementMove() { 
            PieceGuid = pieceGuidToPlace,
            PlayerIndex = 0,
            To = occupiedCoords 
        });
        var controller = new GameSessionController(sessionData, view, input);

        await controller.Turn();
        
        Assert.AreEqual(0, sessionData.CurrentPlayerTurnIndex);
    }

    [Test]
    public async Task Should_PlacePieceOnBoard_When_PlayerMakesPlaceMove()
    {
        var view = Substitute.For<IGameSessionView>();
        var input = Substitute.For<IInputManger>();
        var sessionData = Helpers.CreateStandardSessionStart();
        var pieceToPlace = sessionData.Players[0].UnusedPieces.First.Value;
        var placementCoords = new BoardCoords { X = 1, Y = 1 };
        input.GetMovePiecePlacement(0).Returns(new PlacementMove() {
            PieceGuid = pieceToPlace.Guid,
            PlayerIndex = 0,
            To = placementCoords,
        });
        var controller = new GameSessionController(sessionData, view, input);
        
        await controller.Turn();

        var placedPiece = new BoardService(sessionData.Board).PeekPiece(placementCoords);
        Assert.IsNotNull(placedPiece);
        Assert.AreEqual(pieceToPlace.Guid, placedPiece.Guid);
    }

    [Test]
    public async Task Should_RemovePieceFromPlayer_When_PlayerMakesPlaceMove()
    {
        var sessionData = Helpers.CreateStandardSessionStart();
        var placingPlayer = sessionData.Players[0]; 
        var pieceToPlace = placingPlayer.UnusedPieces.First.Value;
        var placedPieceGuid = pieceToPlace.Guid; 
        var view = Substitute.For<IGameSessionView>();
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
        var view = Substitute.For<IGameSessionView>();
        var input = Substitute.For<IInputManger>();
        var sessionData = new GameSessionData() {
            Board = Helpers.CreateEmptyBoard(),
            State = new GameState() { Stage = Stage.Placement },
        };
        // 0
        var placingPlayer = Helpers.CreatePlayerWithOnePiece(0); 
        sessionData.Players.Add(placingPlayer);
        var pieceToPlaceGuid = placingPlayer.UnusedPieces.First.Value.Guid;
        var placementCoords = new BoardCoords { X = 1, Y = 1 };
        input.GetMovePiecePlacement(0).Returns(new PlacementMove() {
            PieceGuid = pieceToPlaceGuid,
            PlayerIndex = 0,
            To = placementCoords,
        });
        // 1
        placingPlayer = Helpers.CreatePlayerWithOnePiece(1); 
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

    [Test]
    public async Task Should_EndGame_When_PlayerPlacedTicTacToe()
    {
        var view = Substitute.For<IGameSessionView>();
        var sessionData = Helpers.CreateStandardSessionStart();
        var board = new BoardService(sessionData.Board);
        var pieceOwnerId = 0;
        board.PlacePiece(new Piece(pieceOwnerId), new BoardCoords(0, 0));
        board.PlacePiece(new Piece(pieceOwnerId), new BoardCoords(1, 0));
        var pieceToPlaceGuid = sessionData.Players[pieceOwnerId].UnusedPieces.First.Value.Guid;
        var input = Substitute.For<IInputManger>();
        input.GetMovePiecePlacement(pieceOwnerId).Returns(new PlacementMove() {
            PieceGuid = pieceToPlaceGuid,
            PlayerIndex = 0,
            To = new BoardCoords(2, 0),
        });
        var controller = new GameSessionController(sessionData, view, input);
        
        await controller.Turn();
        
        Assert.AreEqual(Stage.End, sessionData.State.Stage);
    }
}