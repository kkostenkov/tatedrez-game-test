using System.Threading.Tasks;
using Tatedrez.Input;
using Tatedrez.Interfaces;
using Tatedrez.ModelServices;
using Tatedrez.Validators;
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
            var inputManager = DI.Game.Resolve<IInputManager>();
            var sessionView = DI.Game.Resolve<GameSessionView>();
            var sessionDataService = DI.Game.Resolve<GameSessionDataService>();

            var commandValidator = DI.Game.Resolve<CommandValidator>();
            
            var gameSessionController = new GameSessionController(data, sessionView, inputManager, 
                commandValidator, sessionDataService);
            
            BindInput(sessionView, inputManager);
            
            await gameSessionController.BuildBoardAsync();
            return gameSessionController;
        }

        private static void BindInput(GameSessionView sessionView, IInputManager inputManager)
        {
            sessionView.BindLocalInputForPlayer(0, inputManager);
            var isBotOpponent = true;
            if (isBotOpponent) {
                var aiInput = DI.Game.Resolve<AiInputManager>();
                inputManager.AddInputSource(aiInput, 1);    
            }
            else {
                sessionView.BindLocalInputForPlayer(1, inputManager);    
            }
        }
    }
}