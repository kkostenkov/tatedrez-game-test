using Tatedrez.Models;

namespace Tatedrez.Views
{
    public interface ISquareView
    {
        BoardCoords Coords { get; }
        Piece Piece { get; }
        void SetHighlightActive(bool active);
    }
}