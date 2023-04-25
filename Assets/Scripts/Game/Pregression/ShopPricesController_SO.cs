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

        
    }
}