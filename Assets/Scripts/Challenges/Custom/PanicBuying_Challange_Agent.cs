using Assets.Scripts.Game.GameMap;
using Assets.Scripts.Game.Items;
using Assets.Scripts.Game.PlayerController;
using Assets.Scripts.Game.Pregression;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Challenges.Custom
{
    public class PanicBuying_Challange_Agent : MonoBehaviour
    {
        private List<Item> boughtItems = new List<Item>();

        PlayerItemsController PlayerItemsController;

        public void OnSetUp(GameProgressionController progression, MainGameLevelMapController main)
        {
            PlayerItemsController = main.Player.itemsController;
            progression.ShopRoomController.ItemBuy.AddListener(OnItemBuy);
            progression.ShopRoomController.OnOpen.AddListener(OnOpen);
        }

        private void OnOpen()
        {
            foreach (Item item in boughtItems)
            {
                PlayerItemsController.RemoveItem(item.name, new Item.ItemExternalEventArgs()
                {
                    mainGameController = MainGameLevelMapController.Instance,
                    player = Player.instance
                });

                Destroy(item.gameObject);
            }

            boughtItems.Clear();


        }

        private void OnItemBuy(Item item)
        {
            boughtItems.Add(item);
        }
    }
}