﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Assets.Scripts.SaveLoad;

namespace Assets.Scripts.Game.SoundManagment
{
    public class GameMusicController : MonoBehaviour
    {
        public AudioSource audioSource;
        
        public AudioClip[] music;
        private List<AudioClip> clipsRandom;

        public bool playOnStart;

        public UnityEvent OnNewSong;

        private float curentSongTime;

        public AudioClip curentSong;

        void Start()
        {
            if (GameSoundController.Instance.curentMusicPlayer == null)
                GameSoundController.Instance.curentMusicPlayer = this;
            else
            {
                Destroy(gameObject);
            }
            clipsRandom = new List<AudioClip>(music);

            SetRandomSong();

            SaveLoadController SaveLoadController = SaveLoadController.Instance;
            if (SaveLoadController != null && SaveLoadController.optionsData != null && SaveLoadController.optionsData.SaveData != null)
            {
                SetVolume(SaveLoadController.optionsData.SaveData.musicVolume);

            }


            if (playOnStart)
            {
                audioSource.Play();
            }
        }

        public void SetMusic(AudioClip song)
        {
            audioSource.clip = song;
            curentSongTime = song.length;
            OnNewSong.Invoke();
        }

        private void SetRandomSong()
        {
            if(clipsRandom.Count <= 0)
            {
                clipsRandom.AddRange(music);

                if(curentSong != null)
                clipsRandom.Remove(curentSong);
            }

            if(clipsRandom.Count == 0)
            {
                curentSongTime = float.MaxValue;
                return;
            }
            
            AudioClip song = clipsRandom[Random.Range(0, clipsRandom.Count)];
            clipsRandom.Remove(song);
            curentSong = song;
            audioSource.clip = song;
            curentSongTime = song.length;
            OnNewSong.Invoke();
        }

        private void Update()
        {
            if(curentSongTime > 0)
            {
                curentSongTime -= Time.deltaTime;
            }
            else
            {
                SetRandomSong();
            }

        }

        public void SetVolume(float value)
        {
            audioSource.volume = value;
        }
    }
}