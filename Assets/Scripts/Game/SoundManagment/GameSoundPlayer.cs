using Assets.Scripts.SaveLoad;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Game.SoundManagment
{
    public class GameSoundPlayer : MonoBehaviour
    {
        public AudioSource audioSource;

        private void Start()
        {
            if(GameSoundController.Instance.curentSoundPlayer == null)
                GameSoundController.Instance.curentSoundPlayer = this;
            else
                Destroy(gameObject);

            SaveLoadController SaveLoadController = SaveLoadController.Instance;
            if (SaveLoadController != null && SaveLoadController.optionsData != null && SaveLoadController.optionsData.SaveData != null)
            {
                SetVolume(SaveLoadController.optionsData.SaveData.soundVolume);
                
            }
        }

        public void SetVolume(float value)
        {
            audioSource.volume = value;
        }

        public void PlaySound(AudioClip clip, float volumeScale)
        {
           audioSource.PlayOneShot(clip, volumeScale);
        }
    }
}