using Assets.Scripts.Game.GameMap;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Game.Pregression.Actions
{
    [CreateAssetMenu(menuName = "Progression/Action_load_MakeBig_Level")]
    public class Progression_Load_And_MakeBig_Level : ProgressionAction
    {
        public Vector2Int newLevelSize;
        public Progresison_LoadLevel loadLevel;

        public override void DoAction(GameProgressionController progression, MainGameLevelMapController main)
        {
            GameMapSizeController.Instance.UpdateToSize(newLevelSize);

            loadLevel.DoAction( progression,  main);
        }

        public override string GetTransitionName(GameProgressionController progression)
        {
            return loadLevel.GetTransitionName(progression);
        }

        
    }
}