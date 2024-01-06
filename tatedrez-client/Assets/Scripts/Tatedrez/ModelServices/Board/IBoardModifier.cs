using Tatedrez.Models;

namespace Tatedrez.ModelServices
{
    public interface IBoardModifier
    {
        void PlacePiece(Piece piece, BoardCoords coords);
        Piece DropPiece(BoardCoords coords);
    }
}