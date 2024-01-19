namespace Tatedrez.Input
{
    public interface IInputSourceCollector
    {
        void AddInputSource(IInputSource input, int playerIndex);
        IInputSource GetInputSource(int playerIndex);
    }
}