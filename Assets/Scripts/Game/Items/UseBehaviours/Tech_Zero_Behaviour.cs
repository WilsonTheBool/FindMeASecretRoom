﻿using Assets.Scripts.Game.Gameplay;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game.Items.UseBehaviours
{
    internal class Tech_Zero_Behaviour: ItemUseBehaviour
    {
        public Sprite bombSelectTileSprite;

        public GameObject lightningObject;

        public float destroyLightningTime;

        public override bool CanUse(Item.ItemInternalEventArgs args)
        {
            return args.external.mainGameController.GameMapRoomUnlockController.CanCheckToUnlock(args.external.tilePos);
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
            if (CanUse(args) && args.item.curentCharge > 0)
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
                range = 2,
                position = args.external.tilePos,
                ExplosionController = args.external.mainGameController.ExplosionController,
                size = new Vector3(1, 1, 1),
                type = Explosion.RangeType.circle,
            };

            explosion.Explode_Fake();


            args.external.mainGameController.GameSelectTileController.ReturnToDefaultSprite();

            Destroy(args.external.mainGameController.LevelMapRenderer.CreateObject(lightningObject, explosion.position), destroyLightningTime);

            ItemUsed.Invoke();
        }
    }
}