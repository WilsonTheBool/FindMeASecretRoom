using Assets.Scripts.Game.Gameplay;
using Assets.Scripts.Game.PlayerController;
using Assets.Scripts.Game.UI;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Game.Items.ItemEffects
{
    public class Create_Pickaxe_Explosion : ItemEffect
    {
        private PlayerHPController playerHPController;

        private SoundManagment.GameSoundController soundManagmentController;

        [SerializeField]
        private int damageOnItemMiss = 1;

        public override void OnEffectAdd(Item.ItemInternalEventArgs args)
        {
            Explosion explosion = new Explosion() { 
                range = 0, 
                position = args.external.tilePos, 
                ExplosionController = args.external.mainGameController.ExplosionController
            };

            playerHPController = args.external.player.playerHPController;

            soundManagmentController = SoundManagment.GameSoundController.Instance;

            explosion.onAfterExplosion += Explosion_onAfterExplosion;

            explosion.Explode_Fake();
        }

        private void Explosion_onAfterExplosion(Explosion arg1, ExplosionResult arg2)
        {
            if(arg2 != null && arg2.secretRoomsUnlocked <= 0) 
            {
                playerHPController.RequestTakeDamage(new PlayerHPController.HpEventArgs(damageOnItemMiss, this.gameObject));
            }
           
        }

        public override void OnEffectRemove(Item.ItemInternalEventArgs args)
        {
            
        }
    }
}