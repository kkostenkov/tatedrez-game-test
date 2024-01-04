using System.Threading.Tasks;
using Tatedrez.Models;

namespace Tatedrez
{
    public class GameSessionController
    {
        private readonly GameSession session;
        private readonly IBoardView boardView;
        private readonly IInputManger input;

        public GameSessionController(GameSession session, IBoardView boardView, IInputManger input)
        {
            this.session = session;
            this.boardView = boardView;
            this.input = input;
        }

        public Task BuildBoardAsync()
        {
            return boardView.Build(session);
        }

        public async Task PlacePieceByPlayer(int playerIndex)
        {
            await boardView.ShowTurn(playerIndex);
            var move = await input.GetMovePiecePlacement(playerIndex); // via input manager?
            // validate move via validator
            
            // check and apply state change
            
            // update pieces view via board view (relay the move?)
            await boardView.VisualizeMove(move);
        }

        public async Task MovePieceByPlayer(int playerIndex)  
        {
            await boardView.ShowTurn(playerIndex);
            // validate if there are available moves
            
            var move = await input.GetMovePieceMovement(playerIndex); // via input manager?
            // validate move via validator
            
            // check and apply state change
            
            // update pieces view via board view
            await boardView.VisualizeMove(move);
        }

        public Task EndGame()
        {
            return boardView.ShowGameOverScreen();
        }
    }
}