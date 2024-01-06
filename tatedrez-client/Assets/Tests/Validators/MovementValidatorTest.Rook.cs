using System.Collections.Generic;
using NUnit.Framework;
using Tatedrez;
using Tatedrez.Models;
using Tatedrez.ModelServices;

namespace MovementValidatorTests
{
    public class MovementValidatorTest_Rook
    {
        private static IEnumerable<TestCaseData> RookValidMoves {
            get {
                yield return new TestCaseData(new BoardCoords(1, 1), new BoardCoords(1, 0)).SetName("1:1 -> 1:0");
                yield return new TestCaseData(new BoardCoords(1, 1), new BoardCoords(1, 2)).SetName("1:1 -> 1:2");
                yield return new TestCaseData(new BoardCoords(1, 1), new BoardCoords(0, 1)).SetName("1:1 -> 0:1");
                yield return new TestCaseData(new BoardCoords(1, 1), new BoardCoords(2, 1)).SetName("1:1 -> 2:1");
            }
        }

        [TestCaseSource(nameof(RookValidMoves))]
        public void Should_AllowMovingRookInAStraightLine_When_FieldIsEmpty(BoardCoords rookCoords,
            BoardCoords destination)
        {
            var board = new BoardService(Helpers.CreateEmptyBoard3by3());
            var rook = new Piece(0) { PieceType = Constants.Rook };
            board.PlacePiece(rook, rookCoords);
            var move = new MovementMove() {
                PieceGuid = rook.Guid,
                From = rookCoords,
                To = destination,
            };
            var validator = new Tatedrez.MovementValidator();

            var result = validator.IsValidMove(board, move);

            Assert.AreEqual(true, result);
        }

        private static IEnumerable<TestCaseData> RookInvalidMoves {
            get {
                yield return new TestCaseData(new BoardCoords(1, 1), new BoardCoords(2, 2)).SetName("1:1 -> 2:2");
                yield return new TestCaseData(new BoardCoords(0, 0), new BoardCoords(1, 2)).SetName("0:0 -> 1:2");
                yield return new TestCaseData(new BoardCoords(1, 1), new BoardCoords(2, 2)).SetName("1:1 -> 2:2");
                yield return new TestCaseData(new BoardCoords(2, 2), new BoardCoords(0, 1)).SetName("2:2 -> 0:1");
            }
        }

        [TestCaseSource(nameof(RookInvalidMoves))]
        public void Should_ProhibitMovingRookNotInAStraightLine_When_FieldIsEmpty(BoardCoords rookCoords,
            BoardCoords destination)
        {
            var board = new BoardService(Helpers.CreateEmptyBoard3by3());
            var rook = new Piece(0) { PieceType = Constants.Rook };
            board.PlacePiece(rook, rookCoords);
            var move = new MovementMove() {
                PieceGuid = rook.Guid,
                From = rookCoords,
                To = destination,
            };
            var validator = new Tatedrez.MovementValidator();

            var result = validator.IsValidMove(board, move);

            Assert.AreEqual(false, result);
        }
    
        private static IEnumerable<TestCaseData> RookJumpOverMoves {
            get {
                yield return new TestCaseData(new BoardCoords(0, 0), new BoardCoords(0, 1), new BoardCoords(0, 2))
                    .SetName("0:0 over 0:1 to 0:2");
                yield return new TestCaseData(new BoardCoords(1, 2), new BoardCoords(1, 1), new BoardCoords(1, 0))
                    .SetName("1:2 over 1:1 to 1:0");
            }
        }

        [TestCaseSource(nameof(RookJumpOverMoves))]
        public void Should_ProhibitMovingRook_When_OtherPieceIsInTheWay(BoardCoords rookCoords, BoardCoords otherPieceCoords, 
            BoardCoords destination)
        {
            var board = new BoardService(Helpers.CreateEmptyBoard3by3());
            var rook = new Piece(0) { PieceType = Constants.Rook };
            board.PlacePiece(rook, rookCoords);
            var otherPiece = new Piece(0);
            board.PlacePiece(otherPiece, otherPieceCoords);
            var move = new MovementMove() {
                PieceGuid = rook.Guid,
                From = rookCoords,
                To = destination,
            };
            var validator = new Tatedrez.MovementValidator();

            var result = validator.IsValidMove(board, move);

            Assert.AreEqual(false, result);
        }
    }
}