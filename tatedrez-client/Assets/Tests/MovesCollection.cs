using System.Collections.Generic;
using NUnit.Framework;
using Tatedrez.Models;

namespace Tatedrez.Tests
{
    public static class MovesCollection
    {
        public static IEnumerable<TestCaseData> StraightLineMoves {
            get {
                yield return new TestCaseData(new BoardCoords(1, 1), new BoardCoords(1, 0)).SetName("1:1 -> 1:0");
                yield return new TestCaseData(new BoardCoords(1, 1), new BoardCoords(1, 2)).SetName("1:1 -> 1:2");
                yield return new TestCaseData(new BoardCoords(1, 1), new BoardCoords(0, 1)).SetName("1:1 -> 0:1");
                yield return new TestCaseData(new BoardCoords(1, 1), new BoardCoords(2, 1)).SetName("1:1 -> 2:1");
                yield return new TestCaseData(new BoardCoords(2, 0), new BoardCoords(2, 2)).SetName("2:0 -> 2:2");
            }
        }
        
        public static IEnumerable<TestCaseData> DiagonalMoves {
            get {
                yield return new TestCaseData(new BoardCoords(1, 1), new BoardCoords(2, 2)).SetName("1:1 -> 2:2");
                yield return new TestCaseData(new BoardCoords(0, 0), new BoardCoords(2, 2)).SetName("0:0 -> 2:2");
                yield return new TestCaseData(new BoardCoords(2, 0), new BoardCoords(0, 2)).SetName("2:0 -> 0:2");
                yield return new TestCaseData(new BoardCoords(2, 0), new BoardCoords(1, 1)).SetName("2:0 -> 1:1");
                yield return new TestCaseData(new BoardCoords(2, 2), new BoardCoords(0, 0)).SetName("2:2 -> 0:0");
            }
        }
    }
}