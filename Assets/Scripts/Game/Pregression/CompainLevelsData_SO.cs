using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Game.Pregression
{
    [CreateAssetMenu(menuName = "Progression/CompainData")]
    public class CompainLevelsData_SO : ScriptableObject
    {
        public CompainLevel[] levels;

        [System.Serializable]
        public struct CompainLevel
        {
            public ProgressionAction levelAction;

            public LevelData level;
        }
    }
}