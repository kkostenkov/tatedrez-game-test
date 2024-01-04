using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using Tatedrez;
using Tatedrez.Models;

public class GameplaySessionControllerTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void GameplaySessionControllerTestSimplePasses()
    {
        // Use the Assert class to test conditions
    }

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
        var pieceToPlace = sessionData.Players[0].UnusedPieces[0];
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

        var placedPiece = sessionData.Board.GetPiece(placementCoords);
        Assert.IsNotNull(placedPiece);
        Assert.AreEqual(pieceToPlace.Guid, placedPiece.Guid);
    }

    public GameSessionData CreateStandardSessionStart()
    {
        return new GameSessionData() {
            Board = new Board() {
                BoardSize = new BoardCoords() {
                    X = 3,
                    Y = 3,
                },
                PiecesByCoordinates = new(),
            },
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

    public List<Piece> CreateStartPieces()
    {
        return new List<Piece>() {
            new Piece() {
                Guid = new Guid(),
                PieceType = "Knight",
            },
            new Piece() {
                Guid = new Guid(),
                PieceType = "Rook",
            },
            new Piece() {
                Guid = new Guid(),
                PieceType = "Bishop",
            },
        };
    }
}