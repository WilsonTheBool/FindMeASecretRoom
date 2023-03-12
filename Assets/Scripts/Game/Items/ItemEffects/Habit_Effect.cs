using Assets.Scripts.Game.PlayerController;
using Assets.Scripts.Game.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Game.Items.ItemEffects
{
    public class Habit_Effect : ItemEffect
    {
        [Range(0f,1f)]
        public float chanceToCharge;

        private PlayerItemsController playerItemsController;

        private GameUIController main;

        public TransferAnimationUIObject cahrgeAnim;

        private Item item;

        public override void OnEffectAdd(Item.ItemInternalEventArgs args)
        {

            this.item = args.item;

            main = GameUIController.Instance;

            playerItemsController = args.external.player.itemsController;

            args.external.player.playerHPController.afterTakeDamage.AddListener(OnTakeDamage);
        }

        public override void OnEffectRemove(Item.ItemInternalEventArgs args)
        {
            args.external.player.playerHPController.afterTakeDamage.RemoveListener(OnTakeDamage);
        }

        void OnTakeDamage(PlayerHPController.HpEventArgs args)
        {
            if(Random.Range(0f,1f) <= chanceToCharge)
            {
                playerItemsController.TryChargeItem(GetNotChargedRandom(), 1);

                //main.PlayerPassiveItemsUIController.StartHIghlightAnim(item);
                //play sound?

                main.CreateTransferAnimation(new GameUIController.TransferAnimData()
                {
                    origin = main.GetPosition_Ui_To_World(main.PlayerPassiveItemsUIController.FindElement(item).transform.position),
                    destination = main.GetPosition_Ui_To_World(main.playerActiveItemsUIController.transform.position),
                    prefab = cahrgeAnim,
                    startScale = new Vector3(0.8f, 0.8f, 1),
                    endScale = new Vector3(0.8f, 0.8f, 1)
                }, 1);
            }
        }

        Item GetNotChargedRandom()
        {
            List<Item> items = new List<Item>();
            foreach(Item item in playerItemsController.ActiveItems)
            {
                if(item.isUseItem && item.isChargeItem && item.curentCharge < item.maxCharge)
                {
                    items.Add(item);
                }
            }


            if(items.Count > 0)
            return items[Random.Range(0,items.Count)];
            else
            return null;
        }
    }
}