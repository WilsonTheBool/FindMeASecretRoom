using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Game.Gameplay
{
    [CreateAssetMenu(menuName = "Game/Explotion Data")]
    public class ExplosionDataHolder_SO : ScriptableObject
    {
        public GameObject ExplosionPrefab;

        public Vector2Int[] GetExplosionRange(int range, Vector2Int pos)
        {
            return TileFigures.GetSquare_Filled(range, pos);
        }
    }
}