using Tatedrez.Models;

namespace Tatedrez.Views
{
    public interface IPlayerView
    {
        Piece SelectedPiece { get; }
        void EnablePieceSelection();
        void DisablePieceSelection();
    }
}