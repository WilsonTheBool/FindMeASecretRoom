using Assets.Scripts.Game.Items;
using Assets.Scripts.Game.PlayerController;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Achievements.Created
{
    [CreateAssetMenu(menuName = "Achievements/Perfection")]
    public class Achievement_Perfection : Achievement_ItemUnlock
    {
        public int LevelsNeeded;

        private int levels;

        private bool isTakenDamageThisLevel = false;

        AchievmentAction.AchivementArgs achivementArgs;

        public override void OnRunStarted(AchievmentAction.AchivementArgs achivementArgs)
        {

            this.achivementArgs = achivementArgs;

            achivementArgs.Player.playerHPController.afterTakeDamage.AddListener(OnDamageCheck);

            achivementArgs.MainGame.onVictory.AddListener(OnLevelComplete);

            base.OnRunStarted(achivementArgs);
        }

        private void OnLevelComplete()
        {
            if (isTakenDamageThisLevel)
            {
               isTakenDamageThisLevel = !isTakenDamageThisLevel;
               levels = 0;
            }
            else
            {
                levels++;
                
                if(levels >= LevelsNeeded)
                {
                    UnlockAchivement(achivementArgs);
                    Unsub();
                }
            }
        }

        private void Unsub()
        {
            achivementArgs.Player.playerHPController.afterTakeDamage.RemoveListener(OnDamageCheck);

            achivementArgs.MainGame.onVictory.RemoveListener(OnLevelComplete);
        }

        public override void OnRunDefeat(AchievmentAction.AchivementArgs achivementArgs)
        {
            Unsub();
        }

        public override void OnRunVictory(AchievmentAction.AchivementArgs achivementArgs)
        {
            Unsub();
        }

        private void OnDamageCheck(PlayerHPController.HpEventArgs args)
        {
            if(args.change != 0)
            {
                isTakenDamageThisLevel = true;
            }
        }
    }
}