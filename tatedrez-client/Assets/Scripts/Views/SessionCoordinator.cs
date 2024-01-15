using System.Threading.Tasks;
using Tatedrez.Views;
using UnityEngine;

namespace Tatedrez
{
    public class SessionCoordinator : MonoBehaviour
    {
        private async void Start()
        {
            var gameSessionController = await PrepareSession();
            
            while (gameSessionController.IsSessionRunning) {
                await gameSessionController.Turn();
            }

            await gameSessionController.Turn();
        }

        private async Task<GameSessionController> PrepareSession()
        {
            var sessionRepo = DI.Container.Resolve<GameSessionRepository>();
            var data = sessionRepo.Load();
            var inputManager = DI.Game.Resolve<PlayerInputManager>();

            var sessionView = DI.Game.Resolve<GameSessionView>();
            sessionView.BindLocalInputForPlayer(0, inputManager);
            sessionView.BindLocalInputForPlayer(1, inputManager);

            var gameSessionController = new GameSessionController(data, sessionView, inputManager, inputManager);
            await gameSessionController.BuildBoardAsync();
            return gameSessionController;
        }
    }
}