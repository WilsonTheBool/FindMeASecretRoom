using System.Collections;
using UnityEngine;
using Assets.Scripts.Game.Gameplay;

namespace Assets.Scripts.Game.Items.ItemEffects
{
    public class MrMega_Effect : ItemEffect
    {
        public int rangeIncrease = 1;
        public float sizeIncreaseMult = 1.25f;
        public override void OnEffectAdd(Item.ItemInternalEventArgs args)
        {
            args.external.mainGameController.ExplosionController.onBeforeExplosion.AddListener(OnExplosion);
        }

        private void OnExplosion(Explosion explosion, ExplosionResult result)
        {
            explosion.range += rangeIncrease;
            explosion.size *= sizeIncreaseMult;
        }

        public override void OnEffectRemove(Item.ItemInternalEventArgs args)
        {
            args.external.mainGameController.ExplosionController.onBeforeExplosion.AddListener(OnExplosion);
        }
    }
}