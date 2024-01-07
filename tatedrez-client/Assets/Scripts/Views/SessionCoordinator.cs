using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;
using Tatedrez.Models;
using Tatedrez.Views;
using UnityEngine;

namespace Tatedrez
{
    public class SessionCoordinator : MonoBehaviour
    {
        [SerializeField]
        private GameSessionView sessionView;

        private GameSessionRepository sessionRepo = new GameSessionRepository();
        private GameSessionController gameSessionController;

        private async void Awake()
        {
            var data = this.sessionRepo.Load();
            var inputManager = new PlayerInputManager();
            this.sessionView.BindLocalInputForPlayer(0, inputManager);
            this.sessionView.BindLocalInputForPlayer(1, inputManager);

            this.gameSessionController = new GameSessionController(data, sessionView, inputManager, inputManager);
            await this.gameSessionController.BuildBoardAsync();
        }

        private async void Start()
        {
            while (this.gameSessionController.IsSessionRunning) {
                await this.gameSessionController.Turn();
            }

            await this.gameSessionController.Turn();
        }
    }
}