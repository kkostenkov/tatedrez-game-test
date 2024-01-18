using NUnit.Framework;
using Tatedrez;
using Tatedrez.Models;
using Tatedrez.ModelServices;
using Tatedrez.Rules;
using Tatedrez.Validators;

namespace MovementValidatorTests
{
    public class MovementValidatorTest
    {
        [Test]
        public void Should_ProhibitMovingPiece_When_TargetCoordsAreOccupied()
        {
            var board = new BoardService();
            board.SetData(Helpers.CreateEmptyBoard3by3());
            var occupiedCoords = new BoardCoords(0, 1);
            board.PlacePiece(new Piece(0), occupiedCoords);
            var movingPiece = new Piece(0);
            var moveStartCoords = new BoardCoords(0, 0);
            board.PlacePiece(movingPiece, moveStartCoords);
            var move = new MovementMove() {
                PieceGuid = movingPiece.Guid,
                From = moveStartCoords,
                To = occupiedCoords,
            };
            var validator = new MovementValidator(new PieceRulesContainer());

            var result = validator.IsValidMove(board, move);

            Assert.AreEqual(false, result);
        }

        [Test]
        public void Should_ProhibitMove_When_MovingFromEmptySquare()
        {
            var board = new BoardService();
            board.SetData(Helpers.CreateEmptyBoard3by3());
            var move = new MovementMove() {
                PieceGuid = System.Guid.NewGuid(),
                From = new BoardCoords(1, 2),
                To = new BoardCoords(2, 2),
            };
            var validator = new MovementValidator(new PieceRulesContainer());

            var result = validator.IsValidMove(board, move);

            Assert.AreEqual(false, result);
        }

        [Test]
        public void Should_ProhibitMove_When_MovingOpponentPiece()
        {
            var board = new BoardService();
            board.SetData(Helpers.CreateEmptyBoard3by3());
            var pieceCoords = new BoardCoords(0, 1);
            var movingPiece = new Piece(1);
            board.PlacePiece(movingPiece, pieceCoords);
            var move = new MovementMove() {
                PlayerIndex = 0,
                PieceGuid = movingPiece.Guid,
                From = pieceCoords,
                To = new BoardCoords(2, 2),
            };
            var validator = new MovementValidator(new PieceRulesContainer());

            var result = validator.IsValidMove(board, move);

            Assert.AreEqual(false, result);
        }
    }
}