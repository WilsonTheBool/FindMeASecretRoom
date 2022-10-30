using System.Collections;
using UnityEngine;
using Assets.Scripts.Game.GameMap;
using Assets.Scripts.Game.Gameplay;
using Assets.Scripts.LevelGeneration;

namespace Assets.Scripts.Game.Items.ItemEffects
{
    public class AnarchistCookbook_Effect : ItemEffect
    {
        public int explosionsCount_min;
        public int explosionsCount_max;

        MainGameLevelMapController main;

        public override void OnEffectAdd(Item.ItemInternalEventArgs args)
        {
            main = args.external.mainGameController;
            main.LevelMapRenderer.onRenderEnded.AddListener(SpawnBombsRandom);
        }

        public override void OnEffectRemove(Item.ItemInternalEventArgs args)
        {
            main.LevelMapRenderer.onRenderEnded.RemoveListener(SpawnBombsRandom);
        }

        private void SpawnBombsRandom()
        {
            var map = main.LevelMap;

            if(map == null)
            {
                Debug.LogError("MAP was null");
                return;
            }

            int count = Random.Range(explosionsCount_min, explosionsCount_max + 1);

            for(int i = 0; i < count; i++)
            {

                Room room = map.rooms[Random.Range(0, map.rooms.Count)];
                Vector2Int bombPos = room.position;

                if (room != null)
                {
                    bombPos += room.Figure.GetRandomRoomTile();
                }

                Explosion explosion = new Explosion() { ExplosionController = main.ExplosionController, position = bombPos, range = 1, type = Explosion.RangeType.square};

                explosion.Explode();
            }

            OnEffectActivated.Invoke();

        }
    }
}