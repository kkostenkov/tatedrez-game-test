using Tatedrez.Models;

namespace Tatedrez.Interfaces
{
    public interface ISquareView
    {
        BoardCoords Coords { get; }
        Piece Piece { get; }
        void SetHighlightActive(bool active);
    }
}