using Assets.Scripts.Game.GameMap;
using Assets.Scripts.Game.Items.ItemPools;
using Assets.Scripts.Game.PlayerController;
using Assets.Scripts.Game.Pregression;
using Assets.Scripts.Game.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game.Items.ItemEffects
{
    public class LostSoul_Effect : ItemEffect
    {
        bool canUse;

        private Item item;

        private ItemPoolController poolController;

        private Player player;

        public override void OnEffectAdd(Item.ItemInternalEventArgs args)
        {
            item = args.item;
            canUse = true;

            player = args.external.player;

            poolController = GameProgressionController.Instance.ItemPoolController;

            args.external.mainGameController.levelStarted.AddListener(OnVictory);
            args.external.mainGameController.onVictory.AddListener(OnNewLevel);

            args.external.player.playerHPController.afterTakeDamage.AddListener(OnTakeDamage);
        }

        public override void OnEffectRemove(Item.ItemInternalEventArgs args)
        {
            args.external.mainGameController.levelStarted.RemoveListener(OnVictory);

            args.external.player.playerHPController.beforeTakeDamage.RemoveListener(OnTakeDamage);
        }

        private void OnVictory()
        {
            
            canUse = true;
            GameUIController.Instance.PlayerPassiveItemsUIController.SetHighlight(item, true);
        }

        private void OnTakeDamage(PlayerHPController.HpEventArgs args)
        {
            if (args.change > 0 && canUse)
            {
                canUse = false;
                GameUIController.Instance.PlayerPassiveItemsUIController.SetHighlight(item, false);
            }
        }

        public void OnNewLevel()
        {
            if (!canUse)
            {
                return;
            }

            List<Item> passiveItems = new List<Item>();

            ItemPool pool = poolController.GetPool(ItemPoolController.PoolType.trasure);

            foreach(Item item in pool.items)
            {
                if (!item.isUseItem)
                {
                    passiveItems.Add(item);
                }
            }

            if(passiveItems.Count > 0)
            {
                Item random = passiveItems[Random.Range(0, passiveItems.Count)];

                player.itemsController.AddItem(random, new Item.ItemExternalEventArgs() { mainGameController = MainGameLevelMapController.Instance, player = player });
                poolController.OnItemPooled(random);
                OnEffectActivated?.Invoke();
            }
            
        }
    }
}