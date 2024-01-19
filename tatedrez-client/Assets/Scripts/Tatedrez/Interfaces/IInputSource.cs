using Tatedrez.ModelServices;
using Tatedrez.Views;

namespace Tatedrez.Input
{
    public interface IInputSource : IMoveFetcher
    {
        void Bind(PlayerService player, IPlayerView playerView, IBoardView boardView);
    }
}