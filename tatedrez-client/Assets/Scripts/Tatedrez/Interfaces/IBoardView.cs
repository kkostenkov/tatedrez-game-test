using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tatedrez.Models;

namespace Tatedrez.Interfaces
{
    public interface IBoardView
    {
        event Action<ISquareView, IBoardView> SquareClicked;
        Task<BoardCoords> GetSelectedSquareAsync();
        void DisableSquaresHighlight();
        void SetHighlighted(IEnumerable<BoardCoords> coords);
    }
}