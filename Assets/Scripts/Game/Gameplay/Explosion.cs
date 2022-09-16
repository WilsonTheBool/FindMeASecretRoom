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
            return ExplosionController.data.GetExplosionRange(range, position);
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