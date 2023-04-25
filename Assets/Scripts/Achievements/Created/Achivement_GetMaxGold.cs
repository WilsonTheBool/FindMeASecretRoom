using Assets.Scripts.Achievements.Actions;
using Assets.Scripts.Game.Items;
using Assets.Scripts.Game.PlayerController;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Achievements.Created
{
    [CreateAssetMenu(menuName = "Achievements/Get 99 gold")]
    public class Achivement_GetMaxGold: Achievement_ItemUnlock
    {
        public int goldToUnlock = 99;

        private AchievmentAction.AchivementArgs args;
        public override void OnRunStarted(AchievmentAction.AchivementArgs achivementArgs)
        {
            args = achivementArgs;
            achivementArgs.Player.goldController.GoldChanged.AddListener(CheckGold);
        }

        private void CheckGold(PlayerGoldController goldController)
        {
            if(goldController.gold == goldToUnlock)
            {
                UnlockAchivement(args);
                UnsubFromGold();   
            }
        }

        private void UnsubFromGold()
        {
            args.Player.goldController.GoldChanged.RemoveListener(CheckGold);
        }
    }
}
