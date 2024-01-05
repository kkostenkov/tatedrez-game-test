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

        private BoardCoords size = BoardCoords.Invalid;

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
            size = boardService.GetSize();
            for (int x = 0; x < size.X; x++) {
                for (int y = 0; y < size.Y; y++) {
                    var coords = new BoardCoords(x, y);
                    var index = ToIndex(coords);
                    this.squares[index].AssignCoords(coords);
                }
            }
            // pieces
            return Task.CompletedTask;
        }

        private int ToIndex(BoardCoords coords)
        {
            var index = coords.X + coords.Y * size.X;
            return index;
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

        public Vector3 GetWorldCoords(BoardCoords coords)
        {
            var index = ToIndex(coords);
            var square = squares[index];
            return square.transform.position;
        }

        public Task PutPiece(Piece piece, BoardCoords destination)
        {
            var index = ToIndex(destination);
            var square = squares[index];
            square.AssignPiece(piece);
            return Task.CompletedTask;
        }
    }
}