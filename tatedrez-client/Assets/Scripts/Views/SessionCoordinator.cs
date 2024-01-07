using System.Threading.Tasks;
using Tatedrez.Views;
using UnityEngine;

namespace Tatedrez
{
    public class SessionCoordinator : MonoBehaviour
    {
        private GameSessionRepository sessionRepo;

        private GameSessionController gameSessionController;

        private async void Start()
        {
            sessionRepo = DI.Container.Resolve<GameSessionRepository>();
            
            await PrepareSession();
            
            while (this.gameSessionController.IsSessionRunning) {
                await this.gameSessionController.Turn();
            }

            await this.gameSessionController.Turn();
        }

        private Task PrepareSession()
        {
            var data = this.sessionRepo.Load();
            var inputManager = DI.Container.Resolve<PlayerInputManager>();

            var sessionView = DI.Container.Resolve<GameSessionView>();
            sessionView.BindLocalInputForPlayer(0, inputManager);
            sessionView.BindLocalInputForPlayer(1, inputManager);

            this.gameSessionController = new GameSessionController(data, sessionView, inputManager, inputManager);
            return this.gameSessionController.BuildBoardAsync();
        }
    }
}