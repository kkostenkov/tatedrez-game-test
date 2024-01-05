using Tatedrez.Models;

namespace Tatedrez.ModelServices
{
    public class GameSessionDataService
    {
        public readonly BoardService BoardService;
        private readonly GameSessionData data;

        public GameSessionDataService(GameSessionData data)
        {
            this.data = data;
            this.BoardService = new BoardService(data.Board);
        }
    }
}