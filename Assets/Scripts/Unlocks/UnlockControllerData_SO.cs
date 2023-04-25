using Assets.Scripts.Challenges;
using Assets.Scripts.Game.Items;
using Assets.Scripts.SaveLoad;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Achievements;

namespace Assets.Scripts.Unlocks
{
    [CreateAssetMenu(menuName = "Progression/Unlock Controller")]
    public class UnlockControllerData_SO : ScriptableObject
    {
        public ChallengeRunController challengeRunController;

        public AchievementsController achievementsController;

        public List<Item> UnlockedItems = new List<Item>();

        public UnityAction<Item> OnNewUnlock;


        public void SetUnlockedItems(ChallengesSaveData saveData)
        {
            //List<Item> unlockedItems = new List<Item>();

            //foreach (ChallengeRunData data in challengeRunController.completedChallenges)
            //{
            //    if (data != null && data.hasUnlockReward)
            //    {
            //        unlockedItems.Add(data.itemUnlock);
            //    }
            //}

            //UnlockedItems = unlockedItems;

            //challengeRunController.ChallengeVictory.AddListener(OnChallengeVictory);
        }

        public void AddUnlockedItems(Item[] items)
        {
            if(UnlockedItems == null)
            {
                UnlockedItems = new List<Item>();
            }

            foreach(Item item in items)
            {
                if (!UnlockedItems.Contains(item))
                {
                    UnlockedItems.Add(item);
                }

            }
            //UnlockedItems.AddRange(items);
        }

        public void AddUnlockedItems(Item item)
        {
            if (UnlockedItems == null)
            {
                UnlockedItems = new List<Item>();
            }
            if (!UnlockedItems.Contains(item))
            {
                UnlockedItems.Add(item);

                OnNewUnlock?.Invoke(item);
            }

            //UnlockedItems.AddRange(items);
        }

        //private void OnChallengeVictory(ChallengeRunData data)
        //{
        //    if(data != null && data.hasUnlockReward)
        //    {
        //        TryUnlockItem(data.itemUnlock);
        //    }
        //}

        public void TryUnlockItem(Item item)
        {
            if (!UnlockedItems.Contains(item))
            {
                UnlockedItems.Add(item);
                OnNewUnlock(item);
            }
        }

    }
}