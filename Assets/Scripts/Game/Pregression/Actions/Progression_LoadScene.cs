using Assets.Scripts.Game.GameMap;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Game.Pregression.Actions
{
    [CreateAssetMenu(menuName = "Progression/Action_loadScene")]
    public class Progression_LoadScene : ProgressionAction
    {
        public string sceneName;

        public override string GetTransitionName(GameProgressionController progression)
        {
            return sceneName;
        }
        public override void DoAction(GameProgressionController progression, MainGameLevelMapController main)
        {
            SceneManager.LoadSceneAsync(sceneName);
        }
    }
}