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
            if(range == 0)
            return new Vector2Int[] {pos};

            if(range >= 1)
            {
                return new Vector2Int[]
                {
                    pos,
                    pos+new Vector2Int(1,0),
                     pos+new Vector2Int(0,1),
                      pos+new Vector2Int(-1,0),
                       pos+new Vector2Int(0,-1),
                        pos+new Vector2Int(1,1),
                         pos+new Vector2Int(1,-1),
                          pos+new Vector2Int(-1,1),
                           pos+new Vector2Int(-1,-1),
                };
            }

            return GetExplosionRange(0, pos);
        }
    }
}