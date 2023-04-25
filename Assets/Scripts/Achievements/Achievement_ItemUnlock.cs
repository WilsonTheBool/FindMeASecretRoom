using Assets.Scripts.Achievements.Actions;
using Assets.Scripts.Game.Items;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Achievements
{
    public class Achievement_ItemUnlock : Achievment
    {
        public Item[] itemsToUnlock;
        public override void OnAwakeWhenUnlocked(AchievmentAction.AchivementArgs achivementArgs)
        {
            UnlockItem(achivementArgs);
            base.OnAwakeWhenUnlocked(achivementArgs);
        }

        private void UnlockItem(AchievmentAction.AchivementArgs achivementArgs)
        {
            foreach(var itemToUnlock in itemsToUnlock)
            {
                new Action_UnlockItem() { itemToUnlock = itemToUnlock }.DoAction(achivementArgs);
            }
            
        }

        public override void UnlockAchivement(AchievmentAction.AchivementArgs achivementArgs)
        {
            UnlockItem(achivementArgs);
            base.UnlockAchivement(achivementArgs);
        }
    }
}