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
            UnlockItem(achivementArgs,false);
            base.OnAwakeWhenUnlocked(achivementArgs);
        }

        private void UnlockItem(AchievmentAction.AchivementArgs achivementArgs, bool showPopup)
        {
            foreach(var itemToUnlock in itemsToUnlock)
            {
                new Action_UnlockItem(itemToUnlock, showPopup).DoAction(achivementArgs);
            }
            
        }

        public override void UnlockAchivement(AchievmentAction.AchivementArgs achivementArgs)
        {
            UnlockItem(achivementArgs,true);
            base.UnlockAchivement(achivementArgs);
        }
    }
}