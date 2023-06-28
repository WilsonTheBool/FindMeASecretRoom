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
        public UnityEvent<Item> ItemBuy;
        public UnityEvent OnCantBuy;

        public UnityEvent OnOpen;
        public UnityEvent OnClose;

        public float saleChance;
        public float saleMult = 0.5f;

        public int maxSaleItems;
        private int curentSaleItems = 0;

        public int shopItemsCount;

        public bool spawnHp = true;

        public int shopIndexCount = 0;

        public bool itemsRestock = false;
        public bool hpRestock = false;

        [HideInInspector]
        public List<PriceData> items = new List<PriceData>();

        private RestockData restockData = new RestockData();

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
            restockData = new RestockData();
            items.Clear();
            curentSaleItems = 0;


            for (int i = 0; i < shopItemsCount; i++)
            {
                Item item = ItemPoolController.GetItemFromPool(ItemPoolController.PoolType.shop);
                bool isSale = false;
                int price = GetItemPrice(item, shopIndexCount, 0, out isSale);

                items.Add(new PriceData(price, item, isSale));
            }

            ShopUIController.CreateWindow(items.ToArray(), this, spawnHp);

            OnOpen.Invoke();

            shopIndexCount++;
        }

        public int GetItemPrice(Item item, int shopIndex, int restockIndex, out bool isSale)
        {
            isSale = false;
            int price = shopPrices.GetPrice(item) + shopPrices.GetPriceIncrease(shopIndex) + shopPrices.GetPriceIncrease_Resotck(restockIndex);
            if (curentSaleItems < maxSaleItems && Random.Range(0f, 1f) < saleChance)
            {
                curentSaleItems++;
                price = Mathf.FloorToInt(price * saleMult);
                isSale = true;
            }

            return price;
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

                if (itemsRestock)
                {
                    ShopUIController.TryGetItemPosition(item, out int index);

                    OnItemBuy.Invoke();
                    ItemBuy.Invoke(item);

                    Item Newitem = ItemPoolController.GetItemFromPool(ItemPoolController.PoolType.shop);

                    int restockCount = restockData.Get(index);

                    PriceData data = new PriceData(GetItemPrice(Newitem, shopIndexCount, restockCount, out bool isSale), Newitem, isSale);
                    items.Add(data);
                    ShopUIController.AddNewItem(data, index);
                    restockCount++;
                    restockData.Set(index, restockCount);
                }
                else
                {
                    OnItemBuy.Invoke();
                    ItemBuy.Invoke(item);
                }


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

        private class RestockData
        {
            public Dictionary<int, int> restockData = new Dictionary<int, int>();

            public void Set(int index, int value)
            {
                if (restockData.ContainsKey(index))
                {
                    restockData[index] = value;
                }
                else
                {
                    restockData.Add(index, value);
                }
            }

            public int Get(int index)
            {
                if (restockData.ContainsKey(index))
                {
                    return restockData[index];
                }

                return 0;
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
