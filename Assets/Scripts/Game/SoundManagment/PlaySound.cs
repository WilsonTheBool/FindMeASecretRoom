using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Game.SoundManagment
{
    public class PlaySound : MonoBehaviour
    {

        public AudioClip clip;

        public float volumeScale;

        public bool playOnStart;

        private void Start()
        {
            if (playOnStart)
            {
                Play();
            }
        }

        public void Play()
        {
            GameSoundController.Instance?.PlayClip(clip, volumeScale, clip.name);
        }
    }
}