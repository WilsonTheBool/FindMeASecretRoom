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

        private OptionsSaveLoadDataHolder options;

        private void Awake()
        {
            if(defaultData == null) defaultData = new GameOptionSaveData();
        }

        void Start()
        {
            SaveLoadController = SaveLoadController.Instance;
            GameSoundController = GameSoundController.Instance;

            SaveLoadController.TryGetSaveLoadComponent(out options);

            if (SoundVolumeSlider != null && SaveLoadController != null && options != null)
            {
                SoundVolumeSlider.value = options.SaveData.soundVolume;
                SoundVolumeSlider.onValueChanged.AddListener(OnSoundValueChanged);
            }
            else
            {
                SoundVolumeSlider.value = defaultData.soundVolume;
                SoundVolumeSlider.onValueChanged.AddListener(OnSoundValueChanged);
            }
            

            if (MusicVolumeSlider != null && SaveLoadController != null && options != null)
            {
                MusicVolumeSlider.value = options.SaveData.musicVolume;
                MusicVolumeSlider.onValueChanged.AddListener(OnMusicValueChanged);
            }
            else
            {
                MusicVolumeSlider.value = defaultData.musicVolume;
                MusicVolumeSlider.onValueChanged.AddListener(OnMusicValueChanged);
            }
                

            if(FullScreenTogle != null && SaveLoadController != null && options != null)
            {
                FullScreenTogle.isOn = options.SaveData.fullscreen;
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
            options.SaveData.soundVolume = value;
        }

        private void OnMusicValueChanged(float value)
        {
            GameSoundController.SetMusicVolume(value);
            options.SaveData.musicVolume = value;
        }

        private void OnFullscreenValueChanged(bool value)
        {
            Screen.fullScreen = value;
            options.SaveData.fullscreen = value;
        }

        public void SaveOptions()
        {
            options.Save_SaveData(SaveLoadController);
        }

        private void OnDisable()
        {
            SaveOptions();
        }
    }
}