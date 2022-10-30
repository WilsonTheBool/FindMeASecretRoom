using Assets.Scripts.Game.GameMap;
using Assets.Scripts.Game.PlayerController;
using Assets.Scripts.Game.UI;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Game.Items.ItemEffects
{
    public class YumHeartEffect : ItemEffect
    {
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
            args.external.mainGameController.levelStarted.AddListener(HealHP);
        }

        public override void OnEffectRemove(Item.ItemInternalEventArgs args)
        {
            args.external.mainGameController.levelStarted.RemoveListener(HealHP);
        }

        public void HealHP()
        {
            if (playerHPController.CanPickUpHeart(redHeart))
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