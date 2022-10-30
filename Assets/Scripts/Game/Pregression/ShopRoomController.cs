using Assets.Scripts.Game.GameMap;
using Assets.Scripts.Game.Items;
using Assets.Scripts.Game.Items.ItemPools;
using Assets.Scripts.Game.PlayerController;
using Assets.Scripts.Game.UI;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Game.Pregression
{
    public class ShopRoomController : MonoBehaviour
    {
        [HideInInspector]
        public PlayerGoldController PlayerGoldController;

        [HideInInspector]
        public PlayerHPController PlayerHPController;

        [HideInInspector]
        public PlayerItemsController PlayerItemsController;

        [HideInInspector]
        public ShopUIController ShopUIController;

        [HideInInspector]
        public ItemPoolController ItemPoolController;

        public HpObject redHp;
        public int redHpPrice;        
        
        public HpObject blueHp;
        public int blueHpPrice;

        public int itemDefaultPrice;

        public UnityEvent OnItemBuy;
        public UnityEvent OnCantBuy;

        public UnityEvent OnOpen;
        public UnityEvent OnClose;

        public float saleChance;

        public int maxSaleItems;
        private int curentSaleItems;

        public int shopItemsCount;

        public bool spawnHp = true;

        [HideInInspector]
        public List<Items.Item> items = new List<Items.Item>();

        private void Start()
        {
            ShopUIController = GameUIController.Instance.ShopUIController;
            ShopUIController.SkipRequested.AddListener(CloseWindow);

            PlayerGoldController = Player.instance.goldController;
            PlayerItemsController = Player.instance.itemsController;
            PlayerHPController = Player.instance.playerHPController;

            ItemPoolController = GameProgressionController.Instance.ItemPoolController;
        }

      

        public void CreateShopWindow()
        {
            
            items.Clear();
            for (int i = 0; i < shopItemsCount; i++)
            {
                items.Add(ItemPoolController.GetItemFromPool(ItemPoolController.PoolType.shop));
            }

            ShopUIController.CreateWindow(items.ToArray(), this, spawnHp);

            OnOpen.Invoke();
        }

        public void CloseWindow()
        {
            ShopUIController.CloseWIndow();

            OnClose.Invoke();
        }

        public bool CanBuy(int cost)
        {
            return PlayerGoldController.CanSpendGold(cost);
        }

        public bool Request_Buy(Item item)
        {
            if (CanBuy(itemDefaultPrice))
            {
                PlayerGoldController.RemoveGold(itemDefaultPrice);
                PlayerItemsController.AddItem(Instantiate(item, PlayerItemsController.transform), 
                    new Item.ItemExternalEventArgs { mainGameController = MainGameLevelMapController.Instance,
                    player = Player.instance,
                    tilePos = Vector2Int.zero,});

                OnItemBuy.Invoke();
                return true;
            }
            else
            {
                OnCantBuy.Invoke();
                return false;
            }
        }

        public int GetPrice(Item item)
        {
            return itemDefaultPrice;
        }

        public int GetPrice(HpObject hpObject)
        {
            if(hpObject == redHp)
            {
                return redHpPrice;
            }
            else
            {
                if(hpObject == blueHp)
                {
                    return blueHpPrice;
                }
                else
                {
                    return 0;
                }
            }
        }

        public bool Request_BuyHeart_Red()
        {
            if (CanBuy(redHpPrice) && PlayerHPController.CanPickUpHeart(redHp))
            {
                PlayerGoldController.RemoveGold(redHpPrice);
                PlayerHPController.RequestPickUpHeart(new PlayerHPController.HpEventArgs(1, redHp, this.gameObject));
                OnItemBuy.Invoke();

                return true;
            }
            else
            {
                OnCantBuy.Invoke();
                return false;
            }
        }

        public bool Request_BuyHeart_Blue()
        {
            if (CanBuy(blueHpPrice) && PlayerHPController.CanPickUpHeart(blueHp))
            {
                PlayerGoldController.RemoveGold(blueHpPrice);
                PlayerHPController.RequestPickUpHeart(new PlayerHPController.HpEventArgs(1, blueHp, this.gameObject));
                OnItemBuy.Invoke();

                return true;
            }
            else
            {
                OnCantBuy.Invoke();
                return false;
            }
        }
    }
}
