using Tatedrez.Audio;
using UnityEngine;

namespace Tatedrez
{
    public class DiBootstrap : MonoBehaviour
    {
        [SerializeField]
        private SoundSpeaker soundSpeaker;

        [SerializeField]
        private ResetButton resetButton;
        
        private void Awake()
        {
            BootstrapDependencyInjection();
            resetButton.Resetting += OnResetting;
        }
        
        private void BootstrapDependencyInjection()
        {
            DI.CreateGameContainer();

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