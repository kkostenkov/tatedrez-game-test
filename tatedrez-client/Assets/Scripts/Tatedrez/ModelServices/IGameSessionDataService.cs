namespace Tatedrez.ModelServices
{
    public interface IGameSessionDataService
    {
        int CurrentTurnNumber { get; }
        int GetPlayersCount { get; }
        int GetCurrentActivePlayerIndex();
        PlayerService GetPlayer(int index);
        BoardService BoardService { get; }
        GameStateService GameStateService { get; }
        EndGameService EndGameService { get; }
    }
}