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

        public ShopPricesController_SO shopPrices;

        public HpObject redHp;
        public int redHpPrice;        
        
        public HpObject blueHp;
        public int blueHpPrice;

        public UnityEvent OnItemBuy;
        public UnityEvent OnCantBuy;

        public UnityEvent OnOpen;
        public UnityEvent OnClose;

        public float saleChance;
        public float saleMult = 0.5f;

        public int maxSaleItems;
        private int curentSaleItems = 0;

        public int shopItemsCount;

        public bool spawnHp = true;

        [HideInInspector]
        public List<PriceData> items = new List<PriceData>();

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
            curentSaleItems = 0;
            for (int i = 0; i < shopItemsCount; i++)
            {
                Item item = ItemPoolController.GetItemFromPool(ItemPoolController.PoolType.shop);
                bool isSale = false;
                int price = shopPrices.GetPrice(item);

                if(curentSaleItems < maxSaleItems && Random.Range(0f,1f) < saleChance)
                {
                    curentSaleItems++;
                    price = Mathf.FloorToInt(price * saleMult);
                    isSale = true;
                }

                items.Add(new PriceData(price, item, isSale));
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
            if (CanBuy(GetPrice(item)))
            {
                PlayerGoldController.RemoveGold(GetPrice(item));
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
            foreach(var price in items)
            {
                if(price.item.Name == item.Name)
                {
                    return price.price;
                }
            }

            return shopPrices.GetPrice(item);
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

        [System.Serializable]
        public struct PriceData
        {
            public int price;
            public Item item;
            public bool isSale;

            public PriceData(int price, Item item, bool isSale)
            {
                this.price = price;
                this.item = item;
                this.isSale = isSale;
            }
        }
    }
}
