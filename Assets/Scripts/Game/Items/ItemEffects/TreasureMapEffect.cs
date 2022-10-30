using Assets.Scripts.Game.GameMap;
using Assets.Scripts.LevelGeneration;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.Game.Items.ItemEffects
{
    public class TreasureMapEffect : ItemEffect
    {
        MainGameLevelMapController main;
        public override void OnEffectAdd(Item.ItemInternalEventArgs args)
        {
            main = args.external.mainGameController;
            main.LevelMapRenderer.onRenderEnded.AddListener(CheckEmptyRooms);
        }

        public override void OnEffectRemove(Item.ItemInternalEventArgs args)
        {
            main.LevelMapRenderer.onRenderEnded.RemoveListener(CheckEmptyRooms);
        }
        
        void CheckEmptyRooms()
        {
            foreach(Room room in main.LevelMap.rooms)
            {
                foreach(Vector2Int blocked in room.Figure.BlockedExits)
                {
                    main.GameTilemapController.SetMarker(blocked + room.position);
                }
            }
        }
    }
}