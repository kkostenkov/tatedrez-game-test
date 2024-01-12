namespace Tatedrez
{
    public interface IInputSourceCollector
    {
        void AddInputSource(IMoveFetcher input, int playerIndex);
    }
}