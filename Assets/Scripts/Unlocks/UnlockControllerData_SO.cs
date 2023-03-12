using Assets.Scripts.Challenges;
using Assets.Scripts.Game.Items;
using Assets.Scripts.SaveLoad;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Unlocks
{
    [CreateAssetMenu(menuName = "Progression/Unlock Controller")]
    public class UnlockControllerData_SO : ScriptableObject
    {
        public ChallengeRunController challengeRunController;

        public List<Item> UnlockedItems;

        public  event UnityAction<Item> OnNewUnlock;

        public void SetUnlockedItems(ChallengesSaveData saveData)
        {
            List<Item> unlockedItems = new List<Item>();

            foreach (ChallengeRunData data in challengeRunController.completedChallenges)
            {
                if (data != null && data.hasUnlockReward)
                {
                    unlockedItems.Add(data.itemUnlock);
                }
            }

            UnlockedItems = unlockedItems;

            challengeRunController.ChallengeVictory.AddListener(OnChallengeVictory);
        }

        private void OnChallengeVictory(ChallengeRunData data)
        {
            if(data != null && data.hasUnlockReward)
            {
                TryUnlockItem(data.itemUnlock);
            }
        }

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