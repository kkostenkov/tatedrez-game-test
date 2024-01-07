using Tatedrez.Audio;
using Tatedrez.Views;
using UnityEngine;

namespace Tatedrez
{
    public class DiBootstrap : MonoBehaviour
    {
        [SerializeField]
        private SoundSpeaker soundSpeaker;

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

            DI.Container.Register<GameSessionRepository>();
            DI.Container.Register<PlayerInputManager>().AsSingleton();
            
            DI.Container.Register<GameSessionView>(sessionView);
            
            InstallAudio();
        }

        private void InstallAudio()
        {
            DI.Container.Register<IPieceSoundPlayer>(soundSpeaker);
        }

        private void OnApplicationQuit()
        {
            UnloadAndCleanAll();
        }
        
        private void OnResetting()
        {
            UnloadAndCleanAll();
        }

        public void UnloadAndCleanAll()
        {
            DI.DisposeOfGameContainer();
        }
    }
}