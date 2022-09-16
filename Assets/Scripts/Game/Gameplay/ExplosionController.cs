﻿using Assets.Scripts.Game.GameMap;
using Assets.Scripts.LevelGeneration;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Game.Gameplay
{
    public class ExplosionController: MonoBehaviour
    {
        public ExplosionDataHolder_SO data;


        public GameMapRoomUnlockController GameMapRoomUnlockController;

        public LevelMapRenderer LevelMapRenderer;

        public float delaySec = 0.2f;

        public ExplosionEvent onBeforeExplosion;
        public ExplosionEvent onAfterExplosion;

        public ExplosionEvent gameObjectExplosionCreated;

        private float curentDelay = 0;

        public LevelMap level;

        private ExplosionQueue queue = new ExplosionQueue();

        private void Update()
        {
            if (curentDelay > 0)
            {
                curentDelay -= Time.deltaTime;
            }
            else
            {

                if (queue != null && queue.Get(out Explosion explosion, out ExplosionResult result))
                {
                    CreateExplosionPrefab(explosion, result);

                    curentDelay = delaySec;
                }
            }
        }

        public void SetUp(LevelMap levelMap)
        {
            this.level = levelMap;
        }

        public ExplosionResult Explode_Fake(Explosion explosion)
        {
            var allPos = explosion.GetCircleGlobal();

            var lockedPos = GameMapRoomUnlockController.GetLockedRoomsPos(allPos);

            ExplosionResult explosionResult = new ExplosionResult()
            {
                areaHit = allPos,
                unlockPositions = lockedPos,
                secretRoomsUnlocked = 0,

            };

            foreach (var pos in lockedPos)
            {
                Room room = level.GetRoom(pos);

                if (room != null)
                {
                    if (GameMapRoomUnlockController.TryUnlockRoom(room))
                    {
                        explosionResult.secretRoomsUnlocked++;
                        LevelMapRenderer.RenderRoom(room);
                    }
                }
            }
            return explosionResult;
        }

        public ExplosionResult Explode(Explosion explosion)
        {
            onBeforeExplosion.Invoke(explosion, null);

            var allPos = explosion.GetCircleGlobal();

            var lockedPos = GameMapRoomUnlockController.GetLockedRoomsPos(allPos);

            ExplosionResult explosionResult = new ExplosionResult()
            {
                areaHit = allPos,
                unlockPositions = lockedPos,
                secretRoomsUnlocked = 0,

            };

            foreach(var pos in lockedPos)
            {
                Room room = level.GetRoom(pos);

                if(room != null)
                {
                    if (GameMapRoomUnlockController.TryUnlockRoom(room))
                    {
                        explosionResult.secretRoomsUnlocked++;
                    }
                }
            }

            queue.Add(explosion, explosionResult);

            

            onAfterExplosion.Invoke(explosion, explosionResult);

            return explosionResult;
        }

        private void CreateExplosionPrefab(Explosion explosion, ExplosionResult explosionResult)
        {
            GameObject gm = LevelMapRenderer.CreateObject(data.ExplosionPrefab, explosion.position, explosion.size);

            explosionResult.explosionObject = gm;

            foreach (var pos in explosionResult.unlockPositions)
            {
                Room room = level.GetRoom(pos);

                if (room != null)
                {

                    LevelMapRenderer.RenderRoom(room);

                }
            }

            gameObjectExplosionCreated.Invoke(explosion, explosionResult);

            Destroy(gm, 1f);
        }


        [System.Serializable]
        public class ExplosionEvent: UnityEvent<Explosion, ExplosionResult>
        {

        }

        private class ExplosionQueue
        {
            private Queue<KeyValuePair<Explosion, ExplosionResult>> queue = new Queue<KeyValuePair<Explosion, ExplosionResult>>();

            public void Add(Explosion explosion, ExplosionResult explosionResult)
            {
                queue.Enqueue(new KeyValuePair<Explosion, ExplosionResult>(explosion, explosionResult));
            }

            public bool IsEmpty()
            {
                return queue.Count <= 0;
            }

            public bool Get(out Explosion explosion, out ExplosionResult result)
            {
                if(queue.Count > 0)
                {
                    var pair = queue.Dequeue();

                    explosion = pair.Key;
                    result = pair.Value;

                    return true;
                }
                else
                {
                    explosion = null;
                    result = null;
                    return false;
                }
            }
        }

    }
}