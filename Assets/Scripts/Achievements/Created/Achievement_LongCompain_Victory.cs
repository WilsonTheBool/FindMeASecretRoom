using Assets.Scripts.Achievements.Actions;
using Assets.Scripts.Challenges;
using Assets.Scripts.Game.Pregression;
using Assets.Scripts.Unlocks;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Achievements.Created
{
    [CreateAssetMenu(menuName = "Achievements/Finish long compain")]
    public class Achievement_LongCompain_Victory : Achievement_ItemUnlock
    {
        public CompainLevelsData_SO longCompain;

        private Action_UnlockChallenge _unlockChallenge;


        public ChallengeRunData[] challengesToUnlock;


        public ItemUnlockedUI ui;

        public override void OnRunVictory(AchievmentAction.AchivementArgs achivementArgs)
        {
            if (achivementArgs.challenges.CurentChallenge == null)
            {
                if (achivementArgs.progression.compainData == longCompain)
                {
                    UnlockAchivement(achivementArgs);

                    base.OnRunVictory(achivementArgs);
                }
            }
        }

        public override void UnlockAchivement(AchievmentAction.AchivementArgs args)
        {
            foreach (ChallengeRunData challenge in challengesToUnlock)
            {
                _unlockChallenge = new Action_UnlockChallenge() { challenge = challenge, ui = ui };
                _unlockChallenge.DoAction(args);
            }

            base.UnlockAchivement(args);
        }
    }
}