using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using Tatedrez;
using Tatedrez.Input;
using Tatedrez.Models;
using Tatedrez.ModelServices;
using Tatedrez.Rules;
using Tatedrez.Validators;

public class GameplaySessionControllerTest
{
    private IGameSessionView view;
    private IInputManager inputManager;
    private ICommandValidator commandValidator;

    [SetUp]
    public void Setup()
    {
        this.view = Substitute.For<IGameSessionView>();
        this.inputManager = Substitute.For<IInputManager>();
        this.commandValidator = Substitute.For<ICommandValidator>();
    }
    
    [TestCase(0, 0, ExpectedResult = 1)]
    [TestCase(10, 0, ExpectedResult = 11)]
    [TestCase(0, 1, ExpectedResult = 1)]
    [TestCase(4, 1, ExpectedResult = 5)]
    public async Task<int> Should_IncrementTurnNumber_When_PlayerPutsPiece(int startTurnNumber, int movingPlayerIndex)
    {
        var sessionData = new GameSessionData {
            CurrentTurn = startTurnNumber,
            Players = new List<Player>() { new Player(), new Player() },
            State = new GameState() {Stage = Stage.Placement },
        };
        var controller = new GameSessionController(sessionData, view, inputManager, new RealCommandValidator(),
            Helpers.CreateDataService());

        await controller.Turn();
        return await Task.FromResult(sessionData.CurrentTurn);
    }

    [TestCase(0, 0, ExpectedResult = 1)]
    [TestCase(10, 0, ExpectedResult = 11)]
    [TestCase(0, 1, ExpectedResult = 1)]
    [TestCase(4, 1, ExpectedResult = 5)]
    public async Task<int> Should_IncrementTurnNumber_When_PlayerMovesPiece(int startTurnNumber, int movingPlayerIndex)
    {
        var sessionData = new GameSessionData {
            CurrentTurn = startTurnNumber,
            Players = new List<Player>() { new Player(), new Player() },
            State = new GameState() { Stage = Stage.Placement },
        };
        var controller = new GameSessionController(sessionData, view, inputManager, new RealCommandValidator(),
            Helpers.CreateDataService());

        await controller.Turn();
        return await Task.FromResult(sessionData.CurrentTurn);
    }
    
    [Test]
    public async Task Should_KeepTurnNumber_When_PlayerMakesInvalidPlacementMove()
    {
        var sessionData = Helpers.CreateStandardSessionStart();
        var occupiedCoords = new BoardCoords(1, 2);
        var boardService = new BoardService();
        boardService.SetData(sessionData.Board);
        boardService.PlacePiece(new Piece(0), occupiedCoords);
        var pieceGuidToPlace = sessionData.Players[0].UnusedPieces.First.Value.Guid;
        inputManager.GetMovePiecePlacement().Returns(new PlacementMove() { 
            PieceGuid = pieceGuidToPlace,
            PlayerIndex = 0,
            To = occupiedCoords 
        });
        var controller = new GameSessionController(sessionData, view, inputManager, commandValidator,
            Helpers.CreateDataService());

        await controller.Turn();
        
        Assert.AreEqual(0, sessionData.CurrentTurn);
    }

    [Test]
    public async Task Should_KeepTurnNumber_When_IncorrectPlayerMakesMove()
    {
        var sessionData = Helpers.CreateStandardSessionStart();
        var movingPlayerIndex = 1;
        sessionData.CurrentTurn = movingPlayerIndex;
        inputManager.GetMovePiecePlacement().Returns(new PlacementMove() { 
            PlayerIndex = 0,
        });
        var controller = new GameSessionController(sessionData, view, inputManager, commandValidator,
            Helpers.CreateDataService());

        await controller.Turn();
        
        Assert.AreEqual(movingPlayerIndex, sessionData.CurrentTurn);
    }
    
    [Test]
    public async Task Should_PlacePieceOnBoard_When_PlayerMakesPlaceMove()
    {
        var sessionData = Helpers.CreateStandardSessionStart();
        var boardService = new BoardService();
        boardService.SetData(sessionData.Board);
        
        var pieceToPlace = sessionData.Players[0].UnusedPieces.First.Value;
        var placementCoords = new BoardCoords { X = 1, Y = 1 };
        inputManager.GetMovePiecePlacement().Returns(new PlacementMove() {
            PieceGuid = pieceToPlace.Guid,
            PlayerIndex = 0,
            To = placementCoords,
        });
        var controller = new GameSessionController(sessionData, view, inputManager, new RealCommandValidator(),
            Helpers.CreateDataService());
        
        await controller.Turn();
        
        var placedPiece = boardService.PeekPiece(placementCoords);
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
        var placementCoords = new BoardCoords { X = 1, Y = 1 };
        inputManager.GetMovePiecePlacement().Returns(new PlacementMove() {
            PieceGuid = pieceToPlace.Guid,
            PlayerIndex = 0,
            To = placementCoords,
        });
        var controller = new GameSessionController(sessionData, view, inputManager, new RealCommandValidator(),
            Helpers.CreateDataService());
        
        await controller.Turn();

        var droppedPiece = placingPlayer.UnusedPieces.FirstOrDefault(p => p.Guid == placedPieceGuid);
        Assert.IsNull(droppedPiece);
    }
    
    [Test]
    public async Task Should_ChangeStageFromPlacementToMovement_When_PlayersHaveNoPieces()
    {
        var sessionData = new GameSessionData() {
            Board = Helpers.CreateEmptyBoard3by3(),
            State = new GameState() { Stage = Stage.Placement },
        };
        // 0
        var placingPlayer = Helpers.CreatePlayerWithOnePiece(0); 
        sessionData.Players.Add(placingPlayer);
        var pieceToPlaceGuid = placingPlayer.UnusedPieces.First.Value.Guid;
        var placementCoords = new BoardCoords { X = 1, Y = 1 };
        var firstPlayerMove = new PlacementMove() {
            PieceGuid = pieceToPlaceGuid,
            PlayerIndex = 0,
            To = placementCoords,
        };
        // 1
        placingPlayer = Helpers.CreatePlayerWithOnePiece(1); 
        sessionData.Players.Add(placingPlayer);
        pieceToPlaceGuid = placingPlayer.UnusedPieces.First.Value.Guid;
        placementCoords = new BoardCoords { X = 2, Y = 2 };
        var secondPlayerMove = new PlacementMove() {
            PieceGuid = pieceToPlaceGuid,
            PlayerIndex = 1,
            To = placementCoords,
        };
        inputManager.GetMovePiecePlacement().Returns(firstPlayerMove, secondPlayerMove);
        var controller = new GameSessionController(sessionData, view, inputManager, new RealCommandValidator(),
            Helpers.CreateDataService());
        
        await controller.Turn();
        await controller.Turn();

        var currentStage = sessionData.State.Stage;
        Assert.AreEqual(Stage.Movement, currentStage);
    }

    [Test]
    public async Task Should_EndGame_When_PlayerPlacedTicTacToe()
    {
        var sessionData = Helpers.CreateStandardSessionStart();
        var board = new BoardService();
        board.SetData(sessionData.Board);
        var pieceOwnerId = 0;
        board.PlacePiece(new Piece(pieceOwnerId), new BoardCoords(0, 0));
        board.PlacePiece(new Piece(pieceOwnerId), new BoardCoords(1, 0));
        var pieceToPlaceGuid = sessionData.Players[pieceOwnerId].UnusedPieces.First.Value.Guid;
        inputManager.GetMovePiecePlacement().Returns(new PlacementMove() {
            PieceGuid = pieceToPlaceGuid,
            PlayerIndex = 0,
            To = new BoardCoords(2, 0),
        });
        var controller = new GameSessionController(sessionData, view, inputManager, new RealCommandValidator(),
            Helpers.CreateDataService());
        
        await controller.Turn();
        
        Assert.AreEqual(Stage.End, sessionData.State.Stage);
    }
}

internal class RealCommandValidator : ICommandValidator
{
    public bool IsValidMove(IBoardInfoService board, PlacementMove move)
    {
        return new BoardValidator().IsValidMove(board, move);
    }

    public bool IsValidMove(IBoardInfoService board, MovementMove move)
    {
        return new MovementValidator(new PieceRulesContainer()).IsValidMove(board, move);
    }

    public List<BoardCoords> TryFindTickTackToe(IBoardInfoService board)
    {
        return new BoardValidator().TryFindTickTackToe(board);
    }
}