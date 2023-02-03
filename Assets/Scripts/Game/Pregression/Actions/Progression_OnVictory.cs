using Assets.Scripts.Game.GameMap;
using Assets.Scripts.Game.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game.Pregression.Actions
{
    [CreateAssetMenu(menuName = "Progression/Action_onVictory")]
    public class Progression_OnVictory: ProgressionAction
    {

        public override void DoAction(GameProgressionController progression, MainGameLevelMapController main)
        {
            progression.transitionEnded.AddListener(OnTransitionEnd);

        }

        void OnTransitionEnd(GameProgressionController p)
        {
            StatisticsController stats = StatisticsController.Instance;
            GameUIController.Instance.VictoryScreenController.SetUp(stats.ItemsCollected.ToArray(), stats.roomsUnlocked.ToArray(),
                stats.levelsCompleted, stats.maxLevelCount);
            p.transitionEnded.RemoveListener(OnTransitionEnd);
            
        }
    }
}
