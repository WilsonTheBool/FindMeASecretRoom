using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Game.Items
{
    [CreateAssetMenu(menuName = "Game/Items/ItemData")]
    public class ItemData : ScriptableObject
    {
        public string ItemName;

        [TextArea]
        public string Description;

        public Sprite ItemSprite;

        public int ItemQuality;
    }
}