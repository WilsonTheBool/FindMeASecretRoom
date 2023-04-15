using Assets.Scripts.Game.Gameplay;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Game.Items.UseBehaviours
{
    public class MamaMega_Behaviour : ItemUseBehaviour
    {
        public Sprite bombSelectTileSprite;

        public override bool CanUse(Item.ItemInternalEventArgs args)
        {
            return args.external.mainGameController.GameMapRoomUnlockController.IsInMapRange(args.external.tilePos);
            //return args.external.mainGameController.GameMapRoomUnlockController.CanCheckToUnlock(args.external.tilePos);
        }

        public override void OnAlternativeUse(Item.ItemInternalEventArgs args)
        {

        }

        public override void OnDeselected(Item.ItemInternalEventArgs args)
        {
            args.external.mainGameController.GameSelectTileController.
               SetPredicate_Default();
            args.external.mainGameController.GameSelectTileController.ReturnToDefaultSprite();
        }

        public override void OnSelected(Item.ItemInternalEventArgs args)
        {
            
            args.external.mainGameController.GameSelectTileController.
                SetPredicate_CanChectTile(args.external.mainGameController.GameMapRoomUnlockController.IsInMapRange);

            OnTilePosChnaged(args);
        }

        public override void OnTilePosChnaged(Item.ItemInternalEventArgs args)
        {
            if (CanUse(args))
            {
                args.external.mainGameController.GameSelectTileController.SetTileSprite(bombSelectTileSprite);
            }
            else
            {
                args.external.mainGameController.GameSelectTileController.SetEmptySprite();
            }
        }

        public override void OnUse(Item.ItemInternalEventArgs args)
        {
            Explosion explosion = new Explosion()
            {
                range = 3,
                position = args.external.tilePos,
                ExplosionController = args.external.mainGameController.ExplosionController,
                size = new Vector3(3, 3, 1),
                type = Explosion.RangeType.square,
            };

            explosion.Explode();


            args.external.mainGameController.GameSelectTileController.ReturnToDefaultSprite();

            ItemUsed.Invoke();

            args.external.player.itemsController.RemoveItem(args.item, args.external);
        }
    }
}