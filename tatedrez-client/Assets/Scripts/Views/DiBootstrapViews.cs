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
            DI.Game.Register<PlayerInputManager>().AsSingleton();
            
            DI.Game.Register<GameSessionView>(sessionView);
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