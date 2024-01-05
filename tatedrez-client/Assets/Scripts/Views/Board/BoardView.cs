using System.Threading.Tasks;
using Tatedrez.ModelServices;
using UnityEngine;

namespace Tatedrez.Views
{
    internal class BoardView : MonoBehaviour
    {
        [SerializeField]
        private SquareView[] squares;
        public async Task BuildBoardAsync(BoardService boardService)
        {
            // board size
            var size = boardService.GetSize();
            for (int x = 0; x < size.X; x++) {
                for (int y = 0; y < size.Y; y++) {
                    var index = x + y * size.X;
                    this.squares[index].SetCoordsText($"{x}:{y}");
                }
            }
            // pieces
        }
    }
}