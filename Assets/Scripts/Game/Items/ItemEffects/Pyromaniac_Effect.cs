using Assets.Scripts.Game.Gameplay;
using Assets.Scripts.Game.PlayerController;
using Assets.Scripts.Game.UI;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Game.Items.ItemEffects
{
    public class Pyromaniac_Effect : ItemEffect
    {

        public float chanceToActivate;

        GameUIController main;
        public HpObject redHeart;
        PlayerHPController playerHPController;

        Item item;

        public TransferAnimationUIObject hpAnim;
        public override void OnEffectAdd(Item.ItemInternalEventArgs args)
        {
            main = GameUIController.Instance;
            item = args.item;
            playerHPController = args.external.player.playerHPController;
            args.external.mainGameController.ExplosionController.onAfterExplosion.AddListener(HealHP);
        }

        public override void OnEffectRemove(Item.ItemInternalEventArgs args)
        {
            args.external.mainGameController.ExplosionController.onAfterExplosion.RemoveListener(HealHP);
        }

        public void HealHP(Explosion explosion, ExplosionResult result)
        {
                if (playerHPController.CanPickUpHeart(redHeart))
                {
                    if (Random.Range(0f, 1f) < chanceToActivate)
                    {

                        playerHPController.RequestPickUpHeart(new PlayerHPController.HpEventArgs(1, redHeart, this.gameObject));

                        main.CreateTransferAnimation(new GameUIController.TransferAnimData()
                        {
                            origin = main.GetPosition_Ui_To_World(main.PlayerPassiveItemsUIController.FindElement(item).transform.position),
                            destination = main.GetPosition_Ui_To_World(main.playerHpUIController.transform.position),
                            prefab = hpAnim,
                            startScale = new Vector3(0.3f, 0.3f, 1),
                            endScale = new Vector3(0.3f, 0.3f, 1)
                        }, 1);
                    }
                }





        }
    }
}