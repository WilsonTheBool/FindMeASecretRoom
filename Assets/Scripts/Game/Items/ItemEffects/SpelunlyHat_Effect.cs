using Assets.Scripts.Game.GameMap;
using Assets.Scripts.Game.Gameplay;
using Assets.Scripts.LevelGeneration;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.Game.Items.ItemEffects
{
    public class SpelunlyHat_Effect : ItemEffect
    {
        public int roomUnlock_min;
        public int roomUnlock_max;

        MainGameLevelMapController main;

        List<Vector2Int> positions;

        public override void OnEffectAdd(Item.ItemInternalEventArgs args)
        {
            positions = new List<Vector2Int>(36);
            main = args.external.mainGameController;
            main.LevelMapRenderer.onRenderEnded.AddListener(CheckEmptyRooms);
        }

        public override void OnEffectRemove(Item.ItemInternalEventArgs args)
        {
            main.LevelMapRenderer.onRenderEnded.RemoveListener(CheckEmptyRooms);
        }

        private void CheckEmptyRooms()
        {

            positions.Clear();
            var map = main.LevelMap;

            if (map == null)
            {
                Debug.LogError("MAP was null");
                return;
            }

            int count = Random.Range(roomUnlock_min, roomUnlock_max + 1);

            

            foreach(Room room in map.rooms)
            {
                if (room != null && (room.type == null || !room.type.isSecretRoom) && room.Figure.RoomExits.Length > 0)
                {
                    foreach(Vector2Int exit in room.Figure.RoomExits)
                    {
                        Vector2Int global = exit + room.position;

                        if (!positions.Contains(global) && map.IsInRange(global))
                        {
                            Room exitRoom = map.GetRoom(global);

                            if (exitRoom == null)
                            {
                                positions.Add(global);
                            }
                        }
                    }

                    foreach (Vector2Int exit in room.Figure.BlockedExits)
                    {
                        Vector2Int global = exit + room.position;

                        if (positions.Contains(global))
                        {
                            positions.Remove(global);
                        }
                    }
                }
            }

            if(count > positions.Count)
            {
                count = positions.Count;
            }

            for(int i = 0; i < count; i++)
            {
                int index = Random.Range(0, positions.Count);

                main.GameTilemapController.SetMarker(positions[index]);

                positions.RemoveAt(index);
            }

        }
    }
}