namespace Tatedrez.Interfaces
{
    public interface IInputSourceCollector
    {
        void AddInputSource(IMoveFetcher input, int playerIndex);
    }
}