using Assets.Scripts.Game.Gameplay;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Game.Items.UseBehaviours
{
    public class MagicFingers_Behaviour : ItemUseBehaviour
    {
        public int cost;
        public Sprite selectTile;

        public override bool CanUse(Item.ItemInternalEventArgs args)
        {
            if (args.external.mainGameController.GameMapRoomUnlockController.IsUnlocked(args.external.tilePos))
            {
                return false;
            }

            return args.external.mainGameController.GameMapRoomUnlockController.CanCheckToUnlock(args.external.tilePos) && args.external.player.goldController.CanSpendGold(cost);
        }

        public override void OnAlternativeUse(Item.ItemInternalEventArgs args)
        {

        }

        public override void OnDeselected(Item.ItemInternalEventArgs args)
        {
            args.external.mainGameController.GameSelectTileController.ReturnToDefaultSprite();
        }

        public override void OnSelected(Item.ItemInternalEventArgs args)
        {
            OnTilePosChnaged(args);
        }

        public override void OnTilePosChnaged(Item.ItemInternalEventArgs args)
        {
            if (CanUse(args))
            {
                args.external.mainGameController.GameSelectTileController.SetTileSprite(selectTile);
            }
            else
            {
                args.external.mainGameController.GameSelectTileController.SetEmptySprite();
            }
        }

        public override void OnUse(Item.ItemInternalEventArgs args)
        {
            args.external.player.goldController.RemoveGold(cost);

            Explosion explosion = new Explosion()
            {
                range = 0,
                position = args.external.tilePos,
                ExplosionController = args.external.mainGameController.ExplosionController,
                size = new Vector3(1, 1, 1),
                type = Explosion.RangeType.square,
            };

            explosion.Explode_Fake();


            args.external.mainGameController.GameSelectTileController.ReturnToDefaultSprite();

            ItemUsed.Invoke();
        }

    }
}