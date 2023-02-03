using Assets.Scripts.Game.Gameplay;
using Assets.Scripts.LevelGeneration;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game.Items.UseBehaviours
{
    public class DadsKey_Behaviour : ItemUseBehaviour
    {
        Room_GM temp;

        public Color change;
        Color saved;

        public Sprite empty;


        public override bool CanUse(Item.ItemInternalEventArgs args)
        {
            return args.external.mainGameController.LevelMap.GetRoom(args.external.tilePos) != null && 
                args.external.mainGameController.GameMapRoomUnlockController.IsUnlocked(args.external.tilePos);
        }

        public override void OnAlternativeUse(Item.ItemInternalEventArgs args)
        {
            return;
        }

        public override void OnDeselected(Item.ItemInternalEventArgs args)
        {
            if (temp != null)
                temp.baseRenderer.color = saved;
        }

        public override void OnSelected(Item.ItemInternalEventArgs args)
        {
            OnTilePosChnaged(args);
        }

        public override void OnTilePosChnaged(Item.ItemInternalEventArgs args)
        {
            if (CanUse(args) && args.item.curentCharge > 0)
            {
                args.external.mainGameController.GameSelectTileController.SetTileSprite(empty);

                if(temp != null)
                    temp.baseRenderer.color = saved;

                if(args.external.mainGameController.LevelMapRenderer.TryGetRoom_GM(args.external.mainGameController.LevelMap.GetRoom(args.external.tilePos).position, out temp))
                {
                    saved = temp.baseRenderer.color;
                    temp.baseRenderer.color = change;
                }
                
            }
            else
            {
                args.external.mainGameController.GameSelectTileController.SetEmptySprite();
                if (temp != null)
                    temp.baseRenderer.color = saved;
                
            }


        }

        public override void OnUse(Item.ItemInternalEventArgs args)
        {
            Room room = args.external.mainGameController.LevelMap.GetRoom(args.external.tilePos);

            Explosion explosion = new Explosion() { ExplosionController = args.external.mainGameController.ExplosionController, 
                position = Vector2Int.zero, range = 0, size = Vector3.one, type = Explosion.RangeType.square };

            foreach(Vector2Int exit in room.Figure.RoomExits)
            {
                Vector2Int pos = exit + room.position;
                explosion.position = pos;
                args.external.mainGameController.ExplosionController.Explode_Fake(explosion);
            }

            args.external.mainGameController.GameSelectTileController.ReturnToDefaultSprite();
            ItemUsed.Invoke();
        }
    }
}
