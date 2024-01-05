using System.Threading.Tasks;
using Tatedrez.Models;
using Tatedrez.ModelServices;
using UnityEngine;

namespace Tatedrez.Views
{
    internal class GameSessionView : MonoBehaviour, IGameSessionView
    {
        [SerializeField]
        private BoardView boardView; 
        
        public async Task Build(GameSessionDataService sessionDataService)
        {
            var board = sessionDataService.BoardService;
            await this.boardView.BuildBoardAsync(board);

            // for each player
            // not placed pieces

            // game stage
            return;
        }

        public Task ShowGameOverScreen()
        {
            throw new System.NotImplementedException();
        }

        public Task VisualizeMove(PlacementMove move)
        {
            throw new System.NotImplementedException();
        }

        public Task VisualizeMove(MovementMove move)
        {
            throw new System.NotImplementedException();
        }

        public Task ShowTurn(int playerIndex)
        {
            throw new System.NotImplementedException();
        }

        public Task VisualizeInvalidMove(PlacementMove move)
        {
            throw new System.NotImplementedException();
        }
    }
}