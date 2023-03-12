using Assets.Scripts.Game.GameMap;
using Assets.Scripts.Game.Items;
using Assets.Scripts.Game.Pregression;
using UnityEngine;

namespace Assets.Scripts.Challenges
{
    [CreateAssetMenu(menuName = "Challenges/Challenge Data")]
    public class ChallengeRunData : ScriptableObject
    {
        public int id;
        public string Name;

        [TextArea(7,14)]
        public string description;

        public bool HideTreasureRooms;
        public bool HideShops;

        public bool OverrrideStartHP;
        public int redHpOnStart;
        public int maxHpContainers;

        public bool AddItemsOnStart;
        public Item[] itemsToAdd;

        public bool RemoveItemsFromPool;
        public Item[] itemsToPool;

        public bool hasUnlockReward;
        public Item itemUnlock;

        public bool removeItemsFromPlayer;
        public Item[] itemsToRemove;

        public bool OverrideGold;
        [Range(0,99)]
        public int GoldOnStart;

        public bool OverrideRoomReward;
        public int baseReward;
        public int rewardPerCombo;

        public bool AddTrinket;
        public Item Trinket;

        public virtual void OnSetUp(GameProgressionController progression, MainGameLevelMapController main)
        {
            return;
        }
    }
}