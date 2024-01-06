using System.Collections.Generic;
using Tatedrez.Models;
using Tatedrez.ModelServices;

namespace Tatedrez.Rules
{
    public class MovesGenerator
    {
        /// From  https://codereview.stackexchange.com/a/237487
        internal static IEnumerable<BoardCoords> GetMoves(BoardCoords position, BoardCoords[] moveTemplates, int range,
            IBoardInfoService board)
        {
            foreach (var moveTemplate in moveTemplates) {
                foreach (var p in GetMovesForTemplate(position, range, board, moveTemplate)) {
                    yield return p;
                }
            }
        }

        private static IEnumerable<BoardCoords> GetMovesForTemplate(BoardCoords position, int range, IBoardInfoService board,
            BoardCoords moveTemplate)
        {
            for (var radius = 1; radius <= range; radius++) {
                foreach (var p in GettemplateMovesForRadius(position, board, moveTemplate, radius)) {
                    yield return p;
                }
            }
        }

        private static IEnumerable<BoardCoords> GettemplateMovesForRadius(BoardCoords position, IBoardInfoService board,
            BoardCoords moveTemplate, int radius)
        {
            var deltaX = radius * moveTemplate.X;
            var deltaY = radius * moveTemplate.Y;
            var newCoords = new BoardCoords(position.X + deltaX, position.Y + deltaY);
            if (board.HasSquare(newCoords)) {
                yield return newCoords;
            }
        }
    }
}