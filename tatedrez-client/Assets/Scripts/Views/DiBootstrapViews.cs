using Tatedrez.ModelServices;
using Tatedrez.Rules;
using Tatedrez.Validators;
using Tatedrez.Views;
using UnityEngine;

namespace Tatedrez
{
    public class DiBootstrapViews : MonoBehaviour
    {
        [SerializeField]
        private ResetButton resetButton;

        [SerializeField]
        private GameSessionView sessionView;

        private void Awake()
        {
            BootstrapDependencyInjection();
            resetButton.Resetting += OnResetting;
        }

        private void BootstrapDependencyInjection()
        {
            DI.CreateGameContainer();

            DI.Game.Register<PieceRulesContainer>().AsSingleton();
            DI.Game.Register<IMovesGenerator, MovesGenerator>().AsSingleton();

            RegisterDataServices();

            RegisterValidators();
            DI.Game.Register<PlayerInputManager>().AsSingleton();

            DI.Game.Register<GameSessionView>(sessionView);
        }

        private static void RegisterDataServices()
        {
            DI.Game.Register<BoardService>().AsSingleton();
            DI.Game.Register<IBoardInfoService>((c, p) => c.Resolve<BoardService>());

            DI.Game.Register<GameStateService>().AsSingleton();
            DI.Game.Register<EndGameService>().AsSingleton();
            DI.Game.Register<IGameSessionDataService, GameSessionDataService>().AsSingleton();
        }

        private static void RegisterValidators()
        {
            DI.Game.Register<IMovementCommandValidator, MovementValidator>().AsSingleton();
            DI.Game.Register<IPlacementCommandValidator, BoardValidator>().AsSingleton();
            DI.Game.Register<CommandValidator>().AsSingleton();
        }

        private void OnApplicationQuit()
        {
            UnloadAndCleanAll();
        }

        private void OnResetting()
        {
            resetButton.Resetting -= OnResetting;
            UnloadAndCleanAll();
        }

        public void UnloadAndCleanAll()
        {
            DI.DisposeOfGameContainer();
        }
    }
}