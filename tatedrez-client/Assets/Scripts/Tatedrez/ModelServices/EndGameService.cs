using Tatedrez.Models;

namespace Tatedrez.ModelServices
{
    public class EndGameService
    {
        private readonly GameSessionData data;

        public EndGameService(GameSessionData data)
        {
            this.data = data;
        }

        public EndGameDetails GetEndGameDetails()
        {
            return this.data.EndGameDetails;
        }
    }
}