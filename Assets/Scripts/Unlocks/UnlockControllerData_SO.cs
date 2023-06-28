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

        public void ClearUnlocks()
        {
            UnlockedItems.Clear();
        }

        public void AddUnlockedItems(Item[] items, bool invokeEvents)
        {
            UnlockedItems ??= new List<Item>();

            foreach(Item item in items)
            {
                if (!UnlockedItems.Contains(item))
                {
                    UnlockedItems.Add(item);
                }

                if (invokeEvents)
                    OnNewUnlock?.Invoke(item);

            }
        }

        public void AddUnlockedItems(Item item, bool invokeEvents)
        {
            UnlockedItems ??= new List<Item>();

            if (!UnlockedItems.Contains(item))
            {
                UnlockedItems.Add(item);

                if(invokeEvents)
                    OnNewUnlock?.Invoke(item);
            }
        }

    }
}