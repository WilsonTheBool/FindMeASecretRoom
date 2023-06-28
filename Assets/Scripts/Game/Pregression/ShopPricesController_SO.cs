using Assets.Scripts.Game.Items;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Game.Pregression
{
    [CreateAssetMenu(menuName = "Shop/ShopPrices_SO")]
    public class ShopPricesController_SO : ScriptableObject
    {
        [SerializeField]
        private int default_price;

        [SerializeField]
        private ShopRoomController.PriceData[] extrodinary_prices;

        [SerializeField]
        private PriceIncrease[] priceIncreases;

        [SerializeField]
        private PriceIncrease[] restockIncrease;

        public int GetPrice(Item item)
        {
            foreach(ShopRoomController.PriceData data in extrodinary_prices)
            {
                if(data.item.Name == item.Name)
                {
                    return data.price;
                }
            }

            return default_price;
        }

        public int GetPriceIncrease(int shopIndex)
        {
            int result = 0;
            foreach(PriceIncrease price in priceIncreases)
            {
                result = price.priceIncrease;
                if (price.shopCount == shopIndex)
                {
                    return result;
                }
            }

            return result;
        }

        public int GetPriceIncrease_Resotck(int restockIndex)
        {
            int result = 0;
            foreach (PriceIncrease price in restockIncrease)
            {
                result = price.priceIncrease;
                if (price.shopCount == restockIndex)
                {
                    return result;
                }
            }

            return result;
        }

        [System.Serializable]
        public struct PriceIncrease
        {
            public int shopCount;

            public int priceIncrease;
        }
    }
}