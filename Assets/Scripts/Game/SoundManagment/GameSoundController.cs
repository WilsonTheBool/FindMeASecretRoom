using Assets.Scripts.Game.GameMap;
using Assets.Scripts.Game.Gameplay;
using Assets.Scripts.Game.Items;
using Assets.Scripts.Game.PlayerController;
using Assets.Scripts.SaveLoad;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game.SoundManagment
{
    public class GameSoundController : MonoBehaviour
    {
        public SoundControllerData data;
        public static GameSoundController Instance { get; private set; }

        [HideInInspector]
        public GameSoundPlayer curentSoundPlayer;

        [HideInInspector]
        public GameMusicController curentMusicPlayer;

        [HideInInspector]
        public MainGameLevelMapController MainGameLevelMapController;

        [HideInInspector]
        public Player player;

        private List<CooldownPair> cooldownList;
        [SerializeField]
        private float cooldown = 0.1f;

        SaveLoadController saveLoadController;

        private void Awake()
        {
            Instance = this;

            cooldownList = new List<CooldownPair>();
        }

        private void Start()
        {
            MainGameLevelMapController = MainGameLevelMapController.Instance;

            player = Player.instance;

            if(MainGameLevelMapController != null)
            {
                MainGameLevelMapController.GameMapRoomUnlockController.roomUnlocked.AddListener(OnRoomUnlocked);
                MainGameLevelMapController.ExplosionController.gameObjectExplosionCreated.AddListener(OnExplosion);
            }

            if(player != null)
            {
                player.playerHPController.afterTakeDamage.AddListener(OnTakeDamage);
                player.itemsController.ActiveItemSwitched.AddListener(OnItemSwitched);
            }

            saveLoadController = SaveLoadController.Instance;
        }

        private void OnItemSwitched(Item item)
        {
            SoundControllerData.GameAudioData audioData = data.player_SwitchItem;
            PlayClip(audioData.GetRandom(), audioData.volumeScale, audioData.dataName);
        }

        private void OnRoomUnlocked(LevelGeneration.Room room)
        {
            if (room.type != null && room.type.isSecretRoom)
            {
                SoundControllerData.GameAudioData audioData = data.breakWall;
                PlayClip(audioData.GetRandom(), audioData.volumeScale, audioData.dataName);
            }
        }

        private void OnTakeDamage(PlayerHPController.HpEventArgs args)
        {
            SoundControllerData.GameAudioData audioData = data.playerHit;
            PlayClip(audioData.GetRandom(), audioData.volumeScale, audioData.dataName);
        }

        private void OnExplosion(Explosion explosion, ExplosionResult result)
        {
            SoundControllerData.GameAudioData audioData = data.bombExplosion;
            PlayClip(audioData.GetRandom(), audioData.volumeScale, audioData.dataName);
        }

        public void SetSoundVolume(float value)
        {
            curentSoundPlayer?.SetVolume(value);
        }

        public void SetMusicVolume(float value)
        {
            curentMusicPlayer?.SetVolume(value);
        }

        public void PlayClip(AudioClip clip, float volumeScale, string id)
        {
            if (!IsInCoolDown(id))
            {
                curentSoundPlayer.PlaySound(clip, volumeScale);
                cooldownList.Add(new CooldownPair(id, cooldown));
            }
                
        }

        public void PlayClip(SoundControllerData.GameAudioData data)
        {
            if (!IsInCoolDown(data.dataName))
            {
                curentSoundPlayer.PlaySound(data.GetRandom(), data.volumeScale);
                cooldownList.Add(new CooldownPair(data.dataName, cooldown));
            }
                
        }

        private bool IsInCoolDown(string id)
        {
            foreach(var pair in cooldownList)
            {
                if(pair.id == id)
                {
                    return true;
                }
            }

            return false;
        }

        private void Update()
        {
            for(int i = 0; i < cooldownList.Count; i++)
            {
                cooldownList[i].cooldown -= Time.deltaTime;

                if(cooldownList[i].cooldown < 0)
                {
                    cooldownList.RemoveAt(i);
                    i--;
                }
            }
        }

        private class CooldownPair
        {
            public string id;
            public float cooldown;

            public CooldownPair(string id, float cooldown)
            {
                this.id = id; this.cooldown = cooldown;
            }
        }
    }
}