using System.Collections.Generic;
using NUnit.Framework;
using Tatedrez;
using Tatedrez.Models;
using Tatedrez.ModelServices;
using Tatedrez.Tests;

namespace MovementValidatorTests
{
    public class MovementValidatorTest_Bishop
    {
        private static IEnumerable<TestCaseData> BishopValidMoves {
            get {
                var iterator = new MovesEnumerator(MovesCollection.DiagonalMoves);
                foreach (var testCaseData in iterator) {
                    yield return testCaseData;
                }
            }
        }
        
        [TestCaseSource(nameof(BishopValidMoves))]
        public void Should_AllowMovingBishopDiagonally_When_FieldIsEmpty(BoardCoords bishopCoords,
            BoardCoords destination)
        {
            var board = new BoardService(Helpers.CreateEmptyBoard3by3());
            var bishop = new Piece(0) { PieceType = Constants.Bishop };
            board.PlacePiece(bishop, bishopCoords);
            var move = new MovementMove() {
                PieceGuid = bishop.Guid,
                From = bishopCoords,
                To = destination,
            };
            var validator = new MovementValidator();

            var result = validator.IsValidMove(board, move);

            Assert.AreEqual(true, result);
        }

        private static IEnumerable<TestCaseData> BishopInvalidMoves => MovesCollection.StraightLineMoves;

        [TestCaseSource(nameof(BishopInvalidMoves))]
        public void Should_ProhibitMovingBishopNotDiagonallyLine_When_FieldIsEmpty(BoardCoords bishopCoords,
            BoardCoords destination)
        {
            var board = new BoardService(Helpers.CreateEmptyBoard3by3());
            var bishop = new Piece(0) { PieceType = Constants.Bishop };
            board.PlacePiece(bishop, bishopCoords);
            var move = new MovementMove() {
                PieceGuid = bishop.Guid,
                From = bishopCoords,
                To = destination,
            };
            var validator = new MovementValidator();

            var result = validator.IsValidMove(board, move);

            Assert.AreEqual(false, result);
        }

        private static IEnumerable<TestCaseData> BishopJumpOverMoves {
            get {
                yield return new TestCaseData(new BoardCoords(0, 0), new BoardCoords(1, 1), new BoardCoords(2, 2))
                    .SetName("0:0 over 1:1 to 2:2");
                yield return new TestCaseData(new BoardCoords(0, 2), new BoardCoords(1, 1), new BoardCoords(2, 0))
                    .SetName("0:2 over 1:1 to 2:0");
                yield return new TestCaseData(new BoardCoords(2, 0), new BoardCoords(1, 1), new BoardCoords(0, 2))
                    .SetName("2:0 over 1:1 to 0:2");
            }
        }

        [TestCaseSource(nameof(BishopJumpOverMoves))]
        public void Should_ProhibitMovingBishop_When_OtherPieceIsInTheWay(BoardCoords bishopCoords,
            BoardCoords otherPieceCoords,
            BoardCoords destination)
        {
            var board = new BoardService(Helpers.CreateEmptyBoard3by3());
            var bishop = new Piece(0) { PieceType = Constants.Bishop };
            board.PlacePiece(bishop, bishopCoords);
            var otherPiece = new Piece(0);
            board.PlacePiece(otherPiece, otherPieceCoords);
            var move = new MovementMove() {
                PieceGuid = bishop.Guid,
                From = bishopCoords,
                To = destination,
            };
            var validator = new MovementValidator();

            var result = validator.IsValidMove(board, move);

            Assert.AreEqual(false, result);
        }
    }
}