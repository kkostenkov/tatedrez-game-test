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
            // pieces
        }
    }
}