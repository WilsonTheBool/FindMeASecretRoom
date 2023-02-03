using Assets.Scripts.Game.GameMap;
using Assets.Scripts.LevelGeneration;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game.Pregression.Actions
{
    [CreateAssetMenu(menuName = "Progression/Action_loadLevel")]
    public class Progresison_LoadLevel: ProgressionAction
    {
        public override string GetTransitionName(GameProgressionController progression)
        {
            return progression.levels[progression.leveldataCount].levelName;
        }

        public override void DoAction(GameProgressionController progression, MainGameLevelMapController main)
        {
            LevelGeneratorParams generatorParams = progression.GetNextLevel().generatorParams;
            main.LevelGenerationController.LevelGeneratorParams = generatorParams;
            progression.transitionEnded.AddListener(OnTransitionEnd);
            progression.transitionFadeInEnd.AddListener(OnTransitionMiddle);
            
        }

        private void OnTransitionMiddle(GameProgressionController progression)
        {
            progression.MainGameLevelMapController.SetUpLevel();
            progression.transitionFadeInEnd.RemoveListener(OnTransitionMiddle);
        }

        private void OnTransitionEnd(GameProgressionController progression)
        {
            progression.MainGameLevelMapController.LevelMapRenderer.StartRenderMap(progression.MainGameLevelMapController.LevelMap);
            progression.transitionEnded.RemoveListener(OnTransitionEnd);
            progression.MainGameLevelMapController.levelStarted.Invoke();
        }
    }
}
