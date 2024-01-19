using Tatedrez.ModelServices;

namespace Tatedrez.Interfaces
{
    public interface IInputSource : IMoveFetcher
    {
        void Bind(PlayerService player, IPlayerView playerView, IBoardView boardView);
    }
}