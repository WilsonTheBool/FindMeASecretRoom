using Assets.Scripts.Game.Gameplay.BossRush;
using Assets.Scripts.LevelGeneration;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Game.Pregression
{
    [CreateAssetMenu(menuName = "Progression/LevelData")]
    public class LevelData : ScriptableObject
    {
        public int id;

        public string levelName;

        public LevelGeneratorParams generatorParams;

        public Color baseColor;

        public bool hasTimer;

        public int timerLength;

        public BossRush_Timer timer_prefab;
    }
}