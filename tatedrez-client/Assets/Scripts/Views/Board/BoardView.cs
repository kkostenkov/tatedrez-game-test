using System.Threading.Tasks;
using Tatedrez.Models;
using Tatedrez.ModelServices;
using UnityEngine;

namespace Tatedrez.Views
{
    internal class BoardView : MonoBehaviour
    {
        [SerializeField]
        private SquareView[] squares;

        private void Awake()
        {
            foreach (var square in this.squares) {
                square.SquareClicked += OnSquareClicked;
            }
        }

        private void OnDestroy()
        {
            foreach (var square in this.squares) {
                square.SquareClicked -= OnSquareClicked;
            }
        }

        public Task BuildBoardAsync(BoardService boardService)
        {
            // board size
            var size = boardService.GetSize();
            for (int x = 0; x < size.X; x++) {
                for (int y = 0; y < size.Y; y++) {
                    var index = x + y * size.X;
                    this.squares[index].AssignCoords(new BoardCoords(x, y));
                }
            }
            // pieces
            return Task.CompletedTask;
        }

        public Task<BoardCoords> GetSelectedSquareAsync()
        {
            this.squareSelectionTaskSource = new();
            return this.squareSelectionTaskSource.Task;
        }

        private TaskCompletionSource<BoardCoords> squareSelectionTaskSource; 

        private void OnSquareClicked(SquareView view)
        {
            if (this.squareSelectionTaskSource == null) {
                return;
            }
            var coords = view.Coords;
            var selectionSource = this.squareSelectionTaskSource; 
            this.squareSelectionTaskSource = null;
            selectionSource.SetResult(coords);
        }
    }
}