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
            
            RegisterValidators();
            DI.Game.Register<PlayerInputManager>().AsSingleton();
            
            DI.Game.Register<GameSessionView>(sessionView);
        }

        private static void RegisterValidators()
        {
            DI.Game.Register<MovementValidator>().AsSingleton();
            DI.Game.Register<BoardValidator>().AsSingleton();
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