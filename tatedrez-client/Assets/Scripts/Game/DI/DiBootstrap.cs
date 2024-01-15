using Tatedrez.Audio;
using UnityEngine;

namespace Tatedrez
{
    public class DiBootstrap : MonoBehaviour
    {
        [SerializeField]
        private SoundSpeaker soundSpeaker;
        
        private void Awake()
        {
            BootstrapDependencyInjection();
        }

        private void BootstrapDependencyInjection()
        {
            DI.Container.Register<GameSessionRepository>().AsSingleton();
            InstallAudio();
        }
    
        private void InstallAudio()
        {
            DI.Container.Register<IPieceSoundPlayer>(soundSpeaker);
        }
    }
}