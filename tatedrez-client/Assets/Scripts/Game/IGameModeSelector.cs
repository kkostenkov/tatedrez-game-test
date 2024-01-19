namespace Tatedrez.ModalViews
{
    public interface IGameModeSelector
    {
        void SetMultiPlayerMode();
        void SetSinglePlayerMode();
        bool IsSinglePlayer { get; }
    }

    public class GameModeSelector : IGameModeSelector
    {
        private bool isSinglePlayer;
        public bool IsSinglePlayer => this.isSinglePlayer;

        public void SetMultiPlayerMode()
        {
            this.isSinglePlayer = false;
        }

        public void SetSinglePlayerMode()
        {
            this.isSinglePlayer = true;
        }
    }
}