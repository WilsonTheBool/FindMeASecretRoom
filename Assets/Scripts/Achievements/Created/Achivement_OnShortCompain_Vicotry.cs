using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Achievements;
using Assets.Scripts.Achievements.Actions;
using Assets.Scripts.Challenges;
using Assets.Scripts.Game.Items;
using Assets.Scripts.Unlocks;

[CreateAssetMenu(menuName = "Achievements/Finish short compain")]
public class Achivement_OnShortCompain_Vicotry : Achievement_ItemUnlock
{
    private Action_UnlockChallenge _unlockChallenge;


    public ChallengeRunData[] challengesToUnlock;


    public ItemUnlockedUI ui;

    public override void OnRunVictory(AchievmentAction.AchivementArgs achivementArgs)
    {
        if(achivementArgs.challenges.CurentChallenge == null)
        {
            UnlockAchivement(achivementArgs);

            base.OnRunVictory(achivementArgs);
        }

       
    }

    public override void UnlockAchivement(AchievmentAction.AchivementArgs args)
    {
        foreach(ChallengeRunData challenge in challengesToUnlock)
        {
            _unlockChallenge = new Action_UnlockChallenge() { challenge = challenge, ui = ui };
            _unlockChallenge.DoAction(args);
        }

        base.UnlockAchivement(args);
    }
}
