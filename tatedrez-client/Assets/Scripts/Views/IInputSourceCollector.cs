namespace Tatedrez.Views
{
    public interface IInputSourceCollector
    {
        void AddInputSource(IMoveFetcher input, int playerIndex);
    }
}