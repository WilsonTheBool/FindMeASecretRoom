using Assets.Scripts.Challenges;
using Assets.Scripts.Unlocks;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Achievements.Actions
{
    public class Action_UnlockChallenge : AchievmentAction
    {
        public ChallengeRunData challenge;

        public ItemUnlockedUI ui;

        public override void DoAction(AchivementArgs args)
        {
            args.challenges.UnlockChallenge(challenge);

            args.GameUIController.InstantiateToCanvas(ui, Game.UI.GameUIController.UIRenderType.OnTopUI).SetUp(
                title: "New Challenge Unlocked!",
                itemName: challenge.Name,
                description: "",
                icon: null
                );
        }

    }
}