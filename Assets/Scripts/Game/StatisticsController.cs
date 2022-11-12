using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Game.Items;
using Assets.Scripts.LevelGeneration;
using Assets.Scripts.Game.Pregression;
using Assets.Scripts.Game.GameMap;
using Assets.Scripts.Game.PlayerController;

namespace Assets.Scripts.Game
{
    public class StatisticsController : MonoBehaviour
    {

        public static StatisticsController Instance;

        public List<Item> ItemsCollected;
        public List<RoomType> roomsUnlocked;

        public int levelsCompleted;
        public int maxLevelCount;

        public GameProgressionController GameProgressionController;
        public MainGameLevelMapController MainGameLevelMapController;
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

            maxLevelCount = GameProgressionController.levels.Length;

            if (MainGameLevelMapController == null)
            {
                MainGameLevelMapController = MainGameLevelMapController.Instance;
            }

            MainGameLevelMapController.GameMapRoomUnlockController.roomUnlocked.AddListener(OnRoomUnlocked);
            MainGameLevelMapController.onVictory.AddListener(() => levelsCompleted++);

            if (Player == null)
            {
                Player = Player.instance;
            }

            Player.itemsController.ItemAdded.AddListener(OnItemAdd);
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