using Assets.Scripts.Game.GameMap;
using Assets.Scripts.Game.Gameplay;
using Assets.Scripts.Game.Items;
using Assets.Scripts.Game.PlayerController;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Game.SoundManagment
{
    public class GameSoundController : MonoBehaviour
    {
        public SoundControllerData data;
        public static GameSoundController Instance { get; private set; }

        public GameSoundPlayer curentSoundPlayer;

        [HideInInspector]
        public MainGameLevelMapController MainGameLevelMapController;

        [HideInInspector]
        public Player player;

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        private void Start()
        {
            MainGameLevelMapController = MainGameLevelMapController.Instance;

            player = Player.instance;


            MainGameLevelMapController.GameMapRoomUnlockController.roomUnlocked.AddListener(OnRoomUnlocked);
            MainGameLevelMapController.ExplosionController.gameObjectExplosionCreated.AddListener(OnExplosion);

            player.playerHPController.afterTakeDamage.AddListener(OnTakeDamage);
            player.itemsController.ActiveItemSwitched.AddListener(OnItemSwitched);
        }

        private void OnItemSwitched(Item item)
        {
            SoundControllerData.GameAudioData audioData = data.player_SwitchItem;
            PlayClip(audioData.GetRandom(), audioData.volumeScale);
        }

        private void OnRoomUnlocked(LevelGeneration.Room room)
        {
            if (room.type != null && room.type.isSecretRoom)
            {
                SoundControllerData.GameAudioData audioData = data.breakWall;
                PlayClip(audioData.GetRandom(), audioData.volumeScale);
            }
        }

        private void OnTakeDamage(PlayerHPController.HpEventArgs args)
        {
            SoundControllerData.GameAudioData audioData = data.playerHit;
            PlayClip(audioData.GetRandom(), audioData.volumeScale);
        }

        private void OnExplosion(Explosion explosion, ExplosionResult result)
        {
            SoundControllerData.GameAudioData audioData = data.bombExplosion;
            PlayClip(audioData.GetRandom(), audioData.volumeScale);
        }

        

        public void PlayClip(AudioClip clip, float volumeScale)
        {
            curentSoundPlayer.PlaySound(clip, volumeScale);
        }

        public void PlayClip(SoundControllerData.GameAudioData data)
        {
            curentSoundPlayer.PlaySound(data.GetRandom(), data.volumeScale);
        }
    }
}