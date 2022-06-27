using UnityEngine;

namespace Assets.Scripts.LevelGeneration
{
    public abstract class LevelGenerator : ScriptableObject
    {
        public abstract bool GenerateLevel(LevelMap levelMap, LevelGeneratorParams data);
    }
}