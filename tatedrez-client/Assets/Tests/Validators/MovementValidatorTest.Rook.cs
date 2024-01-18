using System.Collections.Generic;
using NUnit.Framework;
using Tatedrez;
using Tatedrez.Models;
using Tatedrez.ModelServices;
using Tatedrez.Rules;
using Tatedrez.Tests;

namespace MovementValidatorTests
{
    public class MovementValidatorTest_Rook
    {
        private static IEnumerable<TestCaseData> RookValidMoves => MovesCollection.StraightLineMoves;

        [TestCaseSource(nameof(RookValidMoves))]
        public void Should_AllowMovingRookInAStraightLine_When_FieldIsEmpty(BoardCoords rookCoords,
            BoardCoords destination)
        {
            var board = new BoardService();
            board.SetData(Helpers.CreateEmptyBoard3by3());
            var rook = new Piece(0) { PieceType = Constants.Rook };
            board.PlacePiece(rook, rookCoords);
            var move = new MovementMove() {
                PieceGuid = rook.Guid,
                From = rookCoords,
                To = destination,
            };
            var validator = new Tatedrez.Validators.MovementValidator(new PieceRulesContainer());

            var result = validator.IsValidMove(board, move);

            Assert.AreEqual(true, result);
        }

        private static IEnumerable<TestCaseData> RookInvalidMoves => MovesCollection.DiagonalMoves;

        [TestCaseSource(nameof(RookInvalidMoves))]
        public void Should_ProhibitMovingRookNotInAStraightLine_When_FieldIsEmpty(BoardCoords rookCoords,
            BoardCoords destination)
        {
            var board = new BoardService();
            board.SetData(Helpers.CreateEmptyBoard3by3());
            var rook = new Piece(0) { PieceType = Constants.Rook };
            board.PlacePiece(rook, rookCoords);
            var move = new MovementMove() {
                PieceGuid = rook.Guid,
                From = rookCoords,
                To = destination,
            };
            var validator = new Tatedrez.Validators.MovementValidator(new PieceRulesContainer());

            var result = validator.IsValidMove(board, move);

            Assert.AreEqual(false, result);
        }
    
        private static IEnumerable<TestCaseData> RookJumpOverMoves {
            get {
                yield return new TestCaseData(new BoardCoords(0, 0), new BoardCoords(0, 1), new BoardCoords(0, 2))
                    .SetName("0:0 over 0:1 to 0:2");
                yield return new TestCaseData(new BoardCoords(1, 2), new BoardCoords(1, 1), new BoardCoords(1, 0))
                    .SetName("1:2 over 1:1 to 1:0");
                yield return new TestCaseData(new BoardCoords(0, 2), new BoardCoords(1, 2), new BoardCoords(2, 0))
                    .SetName("0:2 over 1:2 to 2:0");
            }
        }

        [TestCaseSource(nameof(RookJumpOverMoves))]
        public void Should_ProhibitMovingRook_When_OtherPieceIsInTheWay(BoardCoords rookCoords, BoardCoords otherPieceCoords, 
            BoardCoords destination)
        {
            var board = new BoardService();
            board.SetData(Helpers.CreateEmptyBoard3by3());
            var rook = new Piece(0) { PieceType = Constants.Rook };
            board.PlacePiece(rook, rookCoords);
            var otherPiece = new Piece(0);
            board.PlacePiece(otherPiece, otherPieceCoords);
            var move = new MovementMove() {
                PieceGuid = rook.Guid,
                From = rookCoords,
                To = destination,
            };
            var validator = new Tatedrez.Validators.MovementValidator(new PieceRulesContainer());

            var result = validator.IsValidMove(board, move);

            Assert.AreEqual(false, result);
        }
    }
}