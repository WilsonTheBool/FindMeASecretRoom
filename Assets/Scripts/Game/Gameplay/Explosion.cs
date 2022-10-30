using Assets.Scripts.Game.GameMap;
using Assets.Scripts.LevelGeneration;
using System.Collections;
using UnityEngine;
using System;

namespace Assets.Scripts.Game.Gameplay
{
    public class Explosion
    {
        public ExplosionController ExplosionController;

        public Vector2Int position;

        public int range;

        public Vector3 size = new Vector3(1, 1, 1);

        public event Action<Explosion> onBeforeExplosion;
        public event Action<Explosion, ExplosionResult> onAfterExplosion;

        public RangeType type;

        public enum RangeType
        {
            square, circle, cross, circle_filled,
        }

        public void Explode()
        {
            onBeforeExplosion?.Invoke(this);

            ExplosionResult result = ExplosionController.Explode(this);

            onAfterExplosion?.Invoke(this, result);
        }

        public void Explode_Fake()
        {
            onBeforeExplosion?.Invoke(this);

            ExplosionResult result = ExplosionController.Explode_Fake(this);

            onAfterExplosion?.Invoke(this, result);
        }

        public Vector2Int[] GetCircleGlobal()
        {
            if(type == RangeType.circle)
            {
                return TileFigures.GetCircle(range, position);
            }
            else
            {
                if (type == RangeType.square)
                {
                    return TileFigures.GetSquare_Filled(range, position);
                }
                else
                {
                    if (type == RangeType.cross)
                    {
                        return TileFigures.GetCross_Poked(range, position);
                    }
                    else
                    {
                        if (type == RangeType.circle_filled)
                        {
                            return TileFigures.GetCircle_Filled(range, position);
                        }
                        else
                        {
                            return new Vector2Int[0];
                        }
                    }
                }
            }

            
        }
    }

    public class ExplosionResult
    {
        public int secretRoomsUnlocked;

        public Vector2Int[] areaHit;

        public Vector2Int[] unlockPositions;

        public GameObject explosionObject;
    }
}