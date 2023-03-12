using Assets.Scripts.Game.GameMap;
using Assets.Scripts.Game.Pregression;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Challenges
{
    [CreateAssetMenu(menuName = "Progression/Action_Skip")]
    public class SkipTransition_Action : ProgressionAction
    {
        public override void DoAction(GameProgressionController progression, MainGameLevelMapController main)
        {
            progression.LoadNextStep();
        }
    }
}