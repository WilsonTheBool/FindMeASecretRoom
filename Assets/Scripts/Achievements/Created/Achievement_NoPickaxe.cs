using Assets.Scripts.Game.Items;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Achievements.Created
{
    [CreateAssetMenu(menuName = "Achievements/No Pickaxe")]
    public class Achievement_NoPickaxe : Achievement_ItemUnlock
    {
        public Item pick_prefab;

        bool usedPickaxe = false;
        private AchievmentAction.AchivementArgs achivementArgs;
        public override void OnRunStarted(AchievmentAction.AchivementArgs achivementArgs)
        {
            this.achivementArgs = achivementArgs;
            achivementArgs.Player.itemsController.OnItemUsed.AddListener(OnItemUsed);
            achivementArgs.MainGame.onVictory.AddListener(OnLevelComplete);
        }

        private void OnItemUsed(Item item, Item.ItemExternalEventArgs args)
        {
            if(item != null && item.Name == pick_prefab.Name)
            {
                usedPickaxe = true;
            }
        }

        private void OnLevelComplete()
        {
            if (usedPickaxe)
            {
               usedPickaxe = !usedPickaxe;
            }
            else
            {
                UnlockAchivement(achivementArgs);
                Unsub();
            }
        }

        private void Unsub()
        {
            achivementArgs.Player.itemsController.OnItemUsed.RemoveListener(OnItemUsed);
            achivementArgs.MainGame.onVictory.RemoveListener(OnLevelComplete);
        }
    }
}