using UnityEngine;

namespace Assets.Scripts.LevelGeneration
{
    public abstract class LevelGenerator : MonoBehaviour
    {
        public abstract bool GenerateLevel(LevelMap levelMap, LevelGeneratorParams data);
    }
}