using System.Collections.Generic;
using NUnit.Framework;
using Tatedrez;
using Tatedrez.Models;
using Tatedrez.ModelServices;

public class BoardValidatorTest
{
    private static IEnumerable<TestCaseData> HorizontalTicTacToes {
        get {
            yield return new TestCaseData(new List<BoardCoords>()
                    { new BoardCoords(0, 0), new BoardCoords(1, 0), new BoardCoords(2, 0) })
                .SetName("first horizontal line is TicTacToe");
            yield return new TestCaseData(new List<BoardCoords>()
                    { new BoardCoords(0, 1), new BoardCoords(1, 1), new BoardCoords(2, 1) })
                .SetName("second horizontal line is TicTacToe");
            yield return new TestCaseData(new List<BoardCoords>()
                    { new BoardCoords(0, 2), new BoardCoords(1, 2), new BoardCoords(2, 2) })
                .SetName("third horizontal line is TicTacToe");
        }
    }

    [TestCaseSource(nameof(HorizontalTicTacToes))]
    public void Should_DetectTicTacToe_When_HorizontalLineOccupiedByPiecesOfTheSameOwner(List<BoardCoords> coordsList)
    {
        var board = new BoardService(Helpers.CreateEmptyBoard3by3());
        foreach (var coords in coordsList) {
            board.PlacePiece(new Piece(0), coords);
        }

        var boardValidator = new BoardValidator();

        var result = boardValidator.HasTickTackToe(board);
        Assert.AreEqual(true, result);
    }
    
    private static IEnumerable<TestCaseData> VerticalTicTacToes {
        get {
            yield return new TestCaseData(new List<BoardCoords>()
                    { new BoardCoords(0, 0), new BoardCoords(0, 1), new BoardCoords(0, 2) })
                .SetName("first vertical line is TicTacToe");
            yield return new TestCaseData(new List<BoardCoords>()
                    { new BoardCoords(1, 0), new BoardCoords(1, 1), new BoardCoords(1, 2) })
                .SetName("second vertical line is TicTacToe");
            yield return new TestCaseData(new List<BoardCoords>()
                    { new BoardCoords(2, 0), new BoardCoords(2, 1), new BoardCoords(2, 2) })
                .SetName("third vertical line is TicTacToe");
        }
    }

    [TestCaseSource(nameof(VerticalTicTacToes))]
    public void Should_DetectTicTacToe_When_VerticalLineOccupiedByPiecesOfTheSameOwner(List<BoardCoords> coordsList)
    {
        var board = new BoardService(Helpers.CreateEmptyBoard3by3());
        foreach (var coords in coordsList) {
            board.PlacePiece(new Piece(0), coords);
        }

        var boardValidator = new BoardValidator();

        var result = boardValidator.HasTickTackToe(board);
        Assert.AreEqual(true, result);
    }
    
    private static IEnumerable<TestCaseData> DiagonalTicTacToes {
        get {
            yield return new TestCaseData(new List<BoardCoords>()
                    { new BoardCoords(0, 0), new BoardCoords(1, 1), new BoardCoords(2, 2) })
                .SetName("first diagonal line is TicTacToe");
            yield return new TestCaseData(new List<BoardCoords>()
                    { new BoardCoords(0, 2), new BoardCoords(1, 1), new BoardCoords(2, 0) })
                .SetName("second diagonal line is TicTacToe");
        }
    }

    [TestCaseSource(nameof(DiagonalTicTacToes))]
    public void Should_DetectTicTacToe_When_DiagonalLineOccupiedByPiecesOfTheSameOwner(List<BoardCoords> coordsList)
    {
        var board = new BoardService(Helpers.CreateEmptyBoard3by3());
        foreach (var coords in coordsList) {
            board.PlacePiece(new Piece(0), coords);
        }

        var boardValidator = new BoardValidator();

        var result = boardValidator.HasTickTackToe(board);
        Assert.AreEqual(true, result);
    }
    
    [Test]
    public void Should_NotDetectTicTacToe_When_BoardIsEmpty()
    {
        var board = new BoardService(Helpers.CreateEmptyBoard3by3());
        var boardValidator = new BoardValidator();

        var result = boardValidator.HasTickTackToe(board);
        
        Assert.AreEqual(false, result);
    }
    
    private static IEnumerable<TestCaseData> NotTicTacToes {
        get {
            yield return new TestCaseData(new List<BoardCoords>()
                    { new BoardCoords(0, 2)})
                .SetName("One piece");
            yield return new TestCaseData(new List<BoardCoords>()
                    { new BoardCoords(0, 1), new BoardCoords(0, 2) })
                .SetName("Two pieces");
            yield return new TestCaseData(new List<BoardCoords>()
                    { new BoardCoords(1, 1), new BoardCoords(1, 2) })
                .SetName("Two pieces at the end");
            yield return new TestCaseData(new List<BoardCoords>()
                    { new BoardCoords(0, 1), new BoardCoords(1, 2), new BoardCoords(2, 0) })
                .SetName("Scattered");
            yield return new TestCaseData(new List<BoardCoords>()
                    { new BoardCoords(0, 0), new BoardCoords(0, 1), new BoardCoords(1, 1) })
                .SetName("Lower left angle");
        }
    }

    [TestCaseSource(nameof(NotTicTacToes))]
    public void Should_NotDetectTicTacToe_When_BoardDoesNotContainIt(List<BoardCoords> coordsList)
    {
        var board = new BoardService(Helpers.CreateEmptyBoard3by3());
        foreach (var coords in coordsList) {
            board.PlacePiece(new Piece(0), coords);
        }

        var boardValidator = new BoardValidator();

        var result = boardValidator.HasTickTackToe(board);
        Assert.AreEqual(false, result);
    }

    [Test]
    public void Should_AllowPlacingPiece_When_TargetCoordsAreEmpty()
    {
        var board = new BoardService(Helpers.CreateEmptyBoard3by3());
        var move = new PlacementMove() { To = new BoardCoords(0, 2) };
        var validator = new BoardValidator();

        var result = validator.IsValidMove(board, move);
        
        Assert.AreEqual(true, result);
    }
    
    [Test]
    public void Should_ProhibitPlacingPiece_When_TargetCoordsAreOccupied()
    {
        var board = new BoardService(Helpers.CreateEmptyBoard3by3());
        var occupiedCoords = new BoardCoords(1, 1); 
        board.PlacePiece(new Piece(0), occupiedCoords);
        var move = new PlacementMove() { To = occupiedCoords };
        var validator = new BoardValidator();

        var result = validator.IsValidMove(board, move);
        
        Assert.AreEqual(false, result);
    }
}