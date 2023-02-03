using Assets.Scripts.Game.SoundManagment;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.SaveLoad
{
    public class OptionsController : MonoBehaviour
    {
        public Slider SoundVolumeSlider; 
        public Slider MusicVolumeSlider;

        public Toggle FullScreenTogle;
        
        SaveLoadController SaveLoadController;

        GameSoundController GameSoundController;

        public GameOptionSaveData defaultData;

        private void Awake()
        {
            if(defaultData == null) defaultData = new GameOptionSaveData();
        }

        void Start()
        {
            SaveLoadController = SaveLoadController.Instance;
            GameSoundController = GameSoundController.Instance;

            if (SoundVolumeSlider != null && SaveLoadController != null && SaveLoadController.optionsData != null)
            {
                SoundVolumeSlider.value = SaveLoadController.optionsData.SaveData.soundVolume;
                SoundVolumeSlider.onValueChanged.AddListener(OnSoundValueChanged);
            }
            else
            {
                SoundVolumeSlider.value = defaultData.soundVolume;
                SoundVolumeSlider.onValueChanged.AddListener(OnSoundValueChanged);
            }
            

            if (MusicVolumeSlider != null && SaveLoadController != null && SaveLoadController.optionsData != null)
            {
                MusicVolumeSlider.value = SaveLoadController.optionsData.SaveData.musicVolume;
                MusicVolumeSlider.onValueChanged.AddListener(OnMusicValueChanged);
            }
            else
            {
                MusicVolumeSlider.value = defaultData.musicVolume;
                MusicVolumeSlider.onValueChanged.AddListener(OnMusicValueChanged);
            }
                

            if(FullScreenTogle != null && SaveLoadController != null && SaveLoadController.optionsData != null)
            {
                FullScreenTogle.isOn = SaveLoadController.optionsData.SaveData.fullscreen;
                FullScreenTogle.onValueChanged.AddListener(OnFullscreenValueChanged);
            }
            else
            {
                FullScreenTogle.isOn = defaultData.fullscreen;
                FullScreenTogle.onValueChanged.AddListener(OnFullscreenValueChanged);
            }
        }

        private void OnSoundValueChanged(float value)
        {
            GameSoundController.SetSoundVolume(value);
            SaveLoadController.optionsData.SaveData.soundVolume = value;
        }

        private void OnMusicValueChanged(float value)
        {
            GameSoundController.SetMusicVolume(value);
            SaveLoadController.optionsData.SaveData.musicVolume = value;
        }

        private void OnFullscreenValueChanged(bool value)
        {
            Screen.fullScreen = value;
            SaveLoadController.optionsData.SaveData.fullscreen = value;
        }

        public void SaveOptions()
        {
            SaveLoadController.optionsData.SaveOptions();
        }

        private void OnDisable()
        {
            SaveOptions();
        }
    }
}