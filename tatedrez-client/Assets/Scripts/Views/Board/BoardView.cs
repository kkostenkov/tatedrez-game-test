using System.Linq;
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
        private TaskCompletionSource<BoardCoords> squareSelectionTaskSource;
        private ISquareClicksListener clicksListener;

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
            size = boardService.GetSize();
            for (int x = 0; x < size.X; x++) {
                for (int y = 0; y < size.Y; y++) {
                    var coords = new BoardCoords(x, y);
                    var index = ToIndex(coords);
                    var square = this.squares[index];
                    square.AssignCoords(coords);
                    var piece = boardService.PeekPiece(coords);
                    square.AssignPiece(piece);
                }
            }
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

        private void OnSquareClicked(SquareView view)
        {
            if (clicksListener != null) {
                this.clicksListener.OnSquareClicked(view);
            }
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

        public Task<Piece> TakePiece(BoardCoords from)
        {
            var index = ToIndex(from);
            var square = squares[index];
            var piece = square.Piece;
            square.AssignPiece(null);
            return Task.FromResult(piece);
        }

        public Task PutPiece(Piece piece, BoardCoords destination)
        {
            var index = ToIndex(destination);
            var square = squares[index];
            square.AssignPiece(piece);
            return Task.CompletedTask;
        }

        public async Task<MovementMove> GetMove(int playerIndex)
        {
            var moveCollector = new MovePartsCollector();
            var task = moveCollector.WaitForMove(playerIndex);
            this.clicksListener = moveCollector;
            var result = await task;
            this.clicksListener = null;
            return result;
        }

        public Task FlashRed(BoardCoords coords)
        {
            return this.squares[ToIndex(coords)].FlashRedAsync();
        }

        public Task FlashRed()
        {
            var allSquaresFlash = this.squares.Select(s => s.FlashRedAsync());
            return Task.WhenAll(allSquaresFlash);
        }
    }
}