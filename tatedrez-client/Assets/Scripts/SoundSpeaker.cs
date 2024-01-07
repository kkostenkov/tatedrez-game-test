using UnityEngine;

namespace Tatedrez.Audio
{
    public class SoundSpeaker : MonoBehaviour, IPieceSoundPlayer
    {
        [SerializeField]
        private AudioSource audioSource;
    
        public void Play()
        {
            this.audioSource.Play();
        }
    }
}