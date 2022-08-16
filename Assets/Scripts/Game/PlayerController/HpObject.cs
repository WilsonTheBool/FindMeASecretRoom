using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Game.PlayerController
{
    /// <summary>
    /// Hp object data, (red hp containers, blue hp, ...)
    /// </summary>
    [CreateAssetMenu(menuName = "Game/HpController/Hp object data")]
    public class HpObject : ScriptableObject
    {
        public HpType type;

        public Sprite sprite;
    }

    public enum HpType
    {
        red, empty, blue,
    }
}