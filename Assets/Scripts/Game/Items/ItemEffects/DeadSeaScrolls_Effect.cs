using Assets.Scripts.Game.UI;
using Assets.Scripts.Unlocks;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game.Items.ItemEffects
{
    public class DeadSeaScrolls_Effect : ItemEffect
    {
        private List<Item> itemsToMimic;

        public UnlockControllerData_SO UnlockControllerData_SO;

        private Item.ItemInternalEventArgs args;

        private Item instantiatedItem;

        public List<Item> excludingItems;

        public override void OnEffectAdd(Item.ItemInternalEventArgs args)
        {
            itemsToMimic = new List<Item>();

            this.args = args;

            foreach(Item item in args.external.mainGameController.progression.ItemPoolController.default_items.items)
            {
                if(item != null && !item.isUseItem)
                {
                    if(!excludingItems.Contains(item))
                        itemsToMimic.Add(item);
                }
            }

            foreach(Item item in UnlockControllerData_SO.UnlockedItems)
            {
                if (item != null && !item.isUseItem)
                {
                    if (!excludingItems.Contains(item))
                        itemsToMimic.Add(item);
                }
            }

            args.external.mainGameController.LevelMapRenderer.onRenderStarted.AddListener(OnLevelStart);

            args.external.mainGameController.onLevelOver.AddListener(OnLevelEnd);
        }

        private void OnLevelStart()
        {
            instantiatedItem = Instantiate(GetRandomItem(), this.transform);

            foreach(ItemEffect effect in instantiatedItem.onAddItemEffect)
            {
                effect.OnEffectAdd(args);
            }

            //instantiatedItem.OnItemAdd(args.external);

            SetNewIcon(instantiatedItem.Sprite);
        }

        private Item GetRandomItem()
        {
            return itemsToMimic[Random.Range(0,itemsToMimic.Count)];
        }

        private void SetNewIcon(Sprite sprite)
        {
            GameUIController ui = GameUIController.Instance;

            ui.PlayerPassiveItemsUIController.FindElement(args.item).image.sprite = sprite;
        }

        private void OnLevelEnd()
        {
            if(instantiatedItem != null)
            {
                // instantiatedItem.OnItemRemove(args.external);

                foreach (ItemEffect effect in instantiatedItem.onAddItemEffect)
                {
                    effect.OnEffectRemove(args);
                }

                Destroy(instantiatedItem);
                instantiatedItem = null;

                SetNewIcon(args.item.Sprite);
            }
        }

        public override void OnEffectRemove(Item.ItemInternalEventArgs args)
        {
            OnLevelEnd();

            args.external.mainGameController.levelStarted.RemoveListener(OnLevelStart);

            args.external.mainGameController.onLevelOver.RemoveListener(OnLevelEnd);
        }
    }
}