using System;
using System.Threading.Tasks;
using Tatedrez.Models;

namespace Tatedrez.Views
{
    internal interface ISquareClicksListener
    {
        void OnSquareClicked(SquareView view);
    }

    internal class MovePartsCollector : ISquareClicksListener
    {
        private int playerIndexToRecord = -1;
        private TaskCompletionSource<MovementMove> completionSource;

        private Piece selectedPiece;
        private BoardCoords originCoords = BoardCoords.Invalid;
        private BoardCoords destinationCoords = BoardCoords.Invalid;

        public Task<MovementMove> WaitForMove(int playerIndex)
        {
            playerIndexToRecord = playerIndex;
            this.completionSource = new TaskCompletionSource<MovementMove>();
            return this.completionSource.Task;
        }

        public void OnSquareClicked(SquareView view)
        {
            if (!TryRecordMovingPiece(view)) {
                TryRecordDestinationCoords(view.Coords);
            }

            if (TryComposeMove(out var move)) {
                this.completionSource.SetResult(move);
            }
        }

        private bool TryComposeMove(out MovementMove move)
        {
            move = null;
            var isAllDataPresent = selectedPiece != null
                                   && this.originCoords != BoardCoords.Invalid
                                   && this.destinationCoords != BoardCoords.Invalid;
            if (isAllDataPresent) {
                move = new MovementMove() {
                    PieceGuid = selectedPiece.Guid,
                    PlayerIndex = this.playerIndexToRecord,
                    From = this.originCoords,
                    To = this.destinationCoords
                };   
            }

            return isAllDataPresent;
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
    }
}