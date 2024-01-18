using System;
using System.Threading.Tasks;
using Tatedrez.Models;
using Tatedrez.Validators;

namespace Tatedrez.Views
{
    internal interface ISquareClicksListener
    {
        void OnSquareClicked(SquareView view, BoardView boardView);
    }

    internal class MovePartsCollector : ISquareClicksListener, IDisposable
    {
        private int playerIndexToRecord = -1;
        private TaskCompletionSource<MovementMove> completionSource;

        private Piece selectedPiece;
        private BoardCoords originCoords = BoardCoords.Invalid;
        private BoardCoords destinationCoords = BoardCoords.Invalid;
        private SquareView selectedOriginSquare;
        private BoardView boardView;
        private readonly MovementValidator movesGenerator;

        public MovePartsCollector()
        {
            this.movesGenerator = DI.Game.Resolve<MovementValidator>();
        }

        public void Dispose()
        {
            this.boardView.SquareClicked -= OnSquareClicked;
            this.boardView = null;
        }

        public Task<MovementMove> WaitForMove(int playerIndex, BoardView boardView)
        {
            this.boardView = boardView;
            this.boardView.SquareClicked += OnSquareClicked;
            playerIndexToRecord = playerIndex;
            this.completionSource = new TaskCompletionSource<MovementMove>();
            return this.completionSource.Task;
        }

        public void OnSquareClicked(SquareView view, BoardView boardView)
        {
            if (TryRecordMovingPiece(view)) {
                // highlight available moves
            }
            else {
                TryRecordDestinationCoords(view.Coords);
            }

            if (TryComposeMove(out var move)) {
                this.completionSource.SetResult(move);
            }
        }

        private bool TryRecordMovingPiece(SquareView view)
        {
            if (view.Piece == null) {
                return false;
            }

            var piece = view.Piece;
            if (piece.Owner != this.playerIndexToRecord) {
                return false;
            }

            this.selectedPiece = piece;
            if (selectedOriginSquare) {
                selectedOriginSquare.SetHighlightActive(false);    
            }
            this.selectedOriginSquare = view;
            selectedOriginSquare.SetHighlightActive(true);
            this.originCoords = view.Coords;
            return true;
        }

        private bool TryRecordDestinationCoords(BoardCoords coords)
        {
            var isMovingPieceIsSelected = this.selectedPiece != null;
            if (isMovingPieceIsSelected) {
                destinationCoords = coords;
            }

            return isMovingPieceIsSelected;
        }

        private bool TryComposeMove(out MovementMove move)
        {
            move = null;
            var isAllDataPresent = selectedPiece != null
                                   && this.originCoords != BoardCoords.Invalid
                                   && this.destinationCoords != BoardCoords.Invalid;
            if (isAllDataPresent) {
                selectedOriginSquare.SetHighlightActive(false);
                move = new MovementMove() {
                    PieceGuid = selectedPiece.Guid,
                    PlayerIndex = this.playerIndexToRecord,
                    From = this.originCoords,
                    To = this.destinationCoords
                };   
            }

            return isAllDataPresent;
        }
    }
}