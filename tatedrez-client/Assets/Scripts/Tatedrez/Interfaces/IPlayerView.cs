using Tatedrez.Models;

namespace Tatedrez.Interfaces
{
    public interface IPlayerView
    {
        Piece SelectedPiece { get; }
        void EnablePieceSelection();
        void DisablePieceSelection();
    }
}