using Assets.Scripts.Game.Gameplay;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Game.Items.UseBehaviours
{
    public class Technology_1_Behaviour : ItemUseBehaviour
    {
        public Sprite bombSelectTileSprite;
        public Sprite bombSelectTileSprite2;

        public GameObject lightningObject;

        public Vector3 rotationVar1;
        public Vector3 rotationVar2;

        public bool isVar1;

        public float destroyLightningTime;

        public override bool CanUse(Item.ItemInternalEventArgs args)
        {
            return args.external.mainGameController.GameMapRoomUnlockController.CanCheckToUnlock(args.external.tilePos);
        }

        public override void OnAlternativeUse(Item.ItemInternalEventArgs args)
        {
            isVar1 = !isVar1;
            OnTilePosChnaged(args);

            ItemUsed_Alt.Invoke();
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
            if (CanUse(args) && args.item.curentCharge > 0)
            {
                if(isVar1)
                    args.external.mainGameController.GameSelectTileController.SetTileSprite(bombSelectTileSprite);
                else
                    args.external.mainGameController.GameSelectTileController.SetTileSprite(bombSelectTileSprite2);
            }
            else
            {
                args.external.mainGameController.GameSelectTileController.SetEmptySprite();
            }
        }

        public override void OnUse(Item.ItemInternalEventArgs args)
        {
            Explosion.RangeType t;
            if (isVar1)
                t = Explosion.RangeType.line_h;
            else
            {
                t = Explosion.RangeType.line_v;
            }

            Explosion explosion = new Explosion()
            {
                range = 2,
                position = args.external.tilePos,
                ExplosionController = args.external.mainGameController.ExplosionController,
                size = new Vector3(1, 1, 1),
                type = t,
            };

            explosion.Explode_Fake();


            args.external.mainGameController.GameSelectTileController.ReturnToDefaultSprite();

            GameObject anim = args.external.mainGameController.LevelMapRenderer.CreateObject(lightningObject, explosion.position);

            if(isVar1)
                anim.transform.rotation = Quaternion.Euler(rotationVar1);
            else
                anim.transform.rotation = Quaternion.Euler(rotationVar2);

            Destroy(anim, destroyLightningTime);

            ItemUsed.Invoke();
        }
    }
}