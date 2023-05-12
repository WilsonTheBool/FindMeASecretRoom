using Assets.Scripts.Game.GameMap;
using Assets.Scripts.Game.Gameplay;
using Assets.Scripts.LevelGeneration;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game.Items.ItemEffects
{
    public class ScatterBombs_Effect : ItemEffect
    {
        public int minBombs = 1;
        public int maxBombs = 2;

        public override void OnEffectAdd(Item.ItemInternalEventArgs args)
        {
            //args.external.mainGameController.GameMapRoomUnlockController.roomUnlocked.AddListener(OnRoomUnlocked);

            args.external.mainGameController.ExplosionController.onAfterExplosion.AddListener(OnExplosion);
            args.external.mainGameController.ExplosionController.onAfterExplosion_fake.AddListener(OnExplosion);
        }

        private void OnExplosion(Explosion explosion, ExplosionResult result)
        {
            if(result.secretRoomsUnlocked > 0)
            {
                foreach(Vector2Int pos in result.unlockPositions)
                {
                    LevelMap map = MainGameLevelMapController.Instance.LevelMap;

                    if(map != null)
                    {
                        Room room = map.GetRoom(pos);

                        if (room != null && room.type != null && room.type.isSecretRoom)
                        {
                            SpawnBombs(room.position, MainGameLevelMapController.Instance);
                        }
                    }
                }
            }

           
        }

        private void SpawnBombs(Vector2Int pos, MainGameLevelMapController main)
        {
            int randomcount = Random.Range(minBombs, maxBombs + 1);

            List<Vector2Int> neighbours = new List<Vector2Int>();

            #region Finding all neighbours
            Vector2Int neighbour = pos + new Vector2Int(-1,0);
            if(CanUse(main, neighbour))
            {
                neighbours.Add(neighbour);
            }
            neighbour = pos + new Vector2Int(1, 0);
            if (CanUse(main, neighbour))
            {
                neighbours.Add(neighbour);
            }
            neighbour = pos + new Vector2Int(0, 1);
            if (CanUse(main, neighbour))
            {
                neighbours.Add(neighbour);
            }
            neighbour = pos + new Vector2Int(0, -1);
            if (CanUse(main, neighbour))
            {
                neighbours.Add(neighbour);
            }
            neighbour = pos + new Vector2Int(1, 1);
            if (CanUse(main, neighbour))
            {
                neighbours.Add(neighbour);
            }
            neighbour = pos + new Vector2Int(1, -1);
            if (CanUse(main, neighbour))
            {
                neighbours.Add(neighbour);
            }
            neighbour = pos + new Vector2Int(-1, -1);
            if (CanUse(main, neighbour))
            {
                neighbours.Add(neighbour);
            }
            neighbour = pos + new Vector2Int(-1, 1);
            if (CanUse(main, neighbour))
            {
                neighbours.Add(neighbour);
            }
            #endregion

            for(int i = 0; i < randomcount; i++)
            {
                if(neighbours.Count == 0)
                {
                    break;
                }

                Vector2Int nPos = neighbours[Random.Range(0, neighbours.Count)];

                Explosion explosion = new Explosion() { ExplosionController = main.ExplosionController,
                    position = nPos, range = 0, type = Explosion.RangeType.square, size = new Vector3(0.7f,0.7f,0.7f), ignoreCombo = true};

                explosion.Explode(); 

                neighbours.Remove(nPos);
            }
        }

        private bool CanUse(MainGameLevelMapController mainGameController, Vector2Int tilePos)
        {
            if (mainGameController.GameMapRoomUnlockController.IsUnlocked(tilePos))
            {
                return false;
            }

            if (mainGameController.GameTilemapController.IsMarked(tilePos))
            {
                return false;
            }

            return mainGameController.GameMapRoomUnlockController.CanCheckToUnlock(tilePos);
        }

        public override void OnEffectRemove(Item.ItemInternalEventArgs args)
        {
            //args.external.mainGameController.GameMapRoomUnlockController.roomUnlocked.RemoveListener(OnRoomUnlocked);
            args.external.mainGameController.ExplosionController.onAfterExplosion.RemoveListener(OnExplosion);
            args.external.mainGameController.ExplosionController.onAfterExplosion_fake.RemoveListener(OnExplosion);
        }
    }
}