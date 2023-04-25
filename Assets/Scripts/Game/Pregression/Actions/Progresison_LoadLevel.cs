using Assets.Scripts.Game.GameMap;
using Assets.Scripts.Game.Gameplay.BossRush;
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
            return progression.compainData.levels[progression.actionCount].level.levelName;
        }

        public override void DoAction(GameProgressionController progression, MainGameLevelMapController main)
        {
            LevelData levelData = progression.GetCurentLevel();
            LevelGeneratorParams generatorParams = levelData.generatorParams;
            main.LevelGenerationController.LevelGeneratorParams = generatorParams;
            main.LevelGenerationController.levelGenerator = generatorParams.LevelGenerator;
            progression.transitionEnded.AddListener(OnTransitionEnd);
            progression.transitionFadeInEnd.AddListener(OnTransitionMiddle);

            if (levelData.hasTimer)
            {
                if(levelData.timer_prefab != null)
                {
                    Instantiate<BossRush_Timer>(levelData.timer_prefab).SetUp(levelData.timerLength);
                }
                else
                {
                    Debug.LogError("Timer is null in levelData");
                }
            }
            
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
