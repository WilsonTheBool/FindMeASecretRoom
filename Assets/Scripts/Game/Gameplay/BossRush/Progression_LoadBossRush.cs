using System.Collections;
using UnityEngine;
using Assets.Scripts.Game.Pregression;
using Assets.Scripts.Game.GameMap;
using Assets.Scripts.LevelGeneration;

namespace Assets.Scripts.Game.Gameplay.BossRush
{
    [CreateAssetMenu(menuName = "Progression/Load_BossRush")]
    public class Progression_LoadBossRush : ProgressionAction
    {
        public LevelData levelData;

        public LevelGeneratorParams[] GenerationParams;

        public BossRushController prefab;

        private BossRushController bossRush;

        public override string GetTransitionName(GameProgressionController progression)
        {
            return "Boss Rush";
        }

        public override void DoAction(GameProgressionController progression, MainGameLevelMapController main)
        {
            bossRush = Instantiate(prefab);

            bossRush.SetUp(GenerationParams);
        }
    }
}