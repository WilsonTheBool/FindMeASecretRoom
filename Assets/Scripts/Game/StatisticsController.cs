using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Game.Items;
using Assets.Scripts.LevelGeneration;
using Assets.Scripts.Game.Pregression;
using Assets.Scripts.Game.GameMap;
using Assets.Scripts.Game.PlayerController;
using Assets.Scripts.Challenges;

namespace Assets.Scripts.Game
{
    public class StatisticsController : MonoBehaviour
    {

        public static StatisticsController Instance;

        public List<Item> ItemsCollected;
        public List<RoomType> roomsUnlocked;

        public int levelsCompleted;
        public int maxLevelCount;

        [HideInInspector]
        public GameProgressionController GameProgressionController;

        [HideInInspector]
        public MainGameLevelMapController MainGameLevelMapController;

        public ChallengeRunController ChallengeRunController;

        [HideInInspector]
        public Player Player;


        private void Awake()
        {
            Instance = this;
        }

        public void Start()
        {
            if(GameProgressionController == null)
            {
                GameProgressionController = GameProgressionController.Instance;
            }

            if (MainGameLevelMapController == null)
            {
                MainGameLevelMapController = MainGameLevelMapController.Instance;
            }

            MainGameLevelMapController.GameMapRoomUnlockController.roomUnlocked.AddListener(OnRoomUnlocked);
            MainGameLevelMapController.onVictory.AddListener(() => levelsCompleted++);
            ChallengeRunController.ChallengeStarted.AddListener(OnRunStart);


            if (Player == null)
            {
                Player = Player.instance;
            }

            Player.itemsController.ItemAdded.AddListener(OnItemAdd);
        }

        private void OnRunStart(ChallengeRunData data)
        {
            if(data == null || data.Compain == null)
            {
                maxLevelCount = GetLevelCount(ChallengeRunController.default_compain);
            }
            else
            {
                maxLevelCount = GetLevelCount(data.Compain);
            }
        }

        private int GetLevelCount(CompainLevelsData_SO compain)
        {
            int count = 0;

            foreach(var level in compain.levels)
            {
                if(level.level != null)
                {
                    count++;
                }
            }

            return count;
        }

        private void OnRoomUnlocked(Room room)
        {
            if(room.type != null && room.type.isSecretRoom)
            {
                roomsUnlocked.Add(room.type);
            }
        }

        private void OnItemAdd(Item item)
        {
            if(item != null)
            {
                ItemsCollected.Add(item);
            }
        }
    }
}