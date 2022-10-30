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
    }
}