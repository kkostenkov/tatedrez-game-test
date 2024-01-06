using System.Collections.Generic;
using NUnit.Framework;
using Tatedrez;
using Tatedrez.Models;
using Tatedrez.ModelServices;
using Tatedrez.Tests;

namespace MovementValidatorTests
{
    public class MovementValidatorTest_Knight
    {
        private static IEnumerable<TestCaseData> KnightValidMoves => MovesCollection.KnightMoves;

        [TestCaseSource(nameof(KnightValidMoves))]
        public void Should_AllowMovingKnight_When_FieldIsEmpty(BoardCoords knightCoords,
            BoardCoords destination)
        {
            var board = new BoardService(Helpers.CreateEmptyBoard3by3());
            var knight = new Piece(0) { PieceType = Constants.Knight };
            board.PlacePiece(knight, knightCoords);
            var move = new MovementMove() {
                PieceGuid = knight.Guid,
                From = knightCoords,
                To = destination,
            };
            var validator = new MovementValidator();

            var result = validator.IsValidMove(board, move);

            Assert.AreEqual(true, result);
        }
        
        private static IEnumerable<TestCaseData> KnightInvalidMoves {
            get {
                var iterator = new MovesEnumerator(
                    MovesCollection.DiagonalMoves, 
                    MovesCollection.StraightLineMoves);
                foreach (var testCaseData in iterator) {
                    yield return testCaseData;
                }
            }
        }

        [TestCaseSource(nameof(KnightInvalidMoves))]
        public void Should_ProhibitMakingIllegalKnightMoves_When_FieldIsEmpty(BoardCoords knightCoords, BoardCoords destination)
        {
            var board = new BoardService(Helpers.CreateEmptyBoard3by3());
            var knight = new Piece(0) { PieceType = Constants.Knight };
            board.PlacePiece(knight, knightCoords);
            var move = new MovementMove() {
                PieceGuid = knight.Guid,
                From = knightCoords,
                To = destination,
            };
            var validator = new MovementValidator();

            var result = validator.IsValidMove(board, move);

            Assert.AreEqual(false, result);
        }
    }
}