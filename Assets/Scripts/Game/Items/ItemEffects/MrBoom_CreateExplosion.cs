using Assets.Scripts.Game.Gameplay;
using Assets.Scripts.Game.PlayerController;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Game.Items.ItemEffects
{
    public class MrBoom_CreateExplosion : ItemEffect
    {
        public override void OnEffectAdd(Item.ItemInternalEventArgs args)
        {
            Explosion explosion = new Explosion()
            {
                range = 2,
                position = args.external.tilePos,
                ExplosionController = args.external.mainGameController.ExplosionController,
                size = new Vector3(2, 2, 1),
            };

            explosion.Explode();
        }

        public override void OnEffectRemove(Item.ItemInternalEventArgs args)
        {
           
        }
    }
}