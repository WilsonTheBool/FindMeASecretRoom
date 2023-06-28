using Assets.Scripts.Game.GameMap;
using Assets.Scripts.Game.Items;
using Assets.Scripts.Game.PlayerController;
using Assets.Scripts.Game.Pregression;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Challenges.Custom
{
    [CreateAssetMenu(menuName = "Challenges/PanicBuying Challenge Data")]
    public class PanicBuying_challenge_data : ChallengeRunData
    {
        public PanicBuying_Challange_Agent prefab;

        public override void OnSetUp(GameProgressionController progression, MainGameLevelMapController main)
        {
            Instantiate(prefab).OnSetUp(progression,main);

            base.OnSetUp(progression, main);
        }

       
    }
}