using UnityEngine;

namespace Assets.Scripts.LevelGeneration
{
    public abstract class LevelGenerator : MonoBehaviour
    {
        public const int StartRoomX = 7;
        public const int StartRoomY = 7;

        public abstract bool GenerateLevel(LevelMap levelMap, LevelGeneratorParams data);
    }
}