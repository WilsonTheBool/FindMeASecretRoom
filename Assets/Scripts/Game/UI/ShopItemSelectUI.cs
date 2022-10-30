using Assets.Scripts.Game.PlayerController;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.Game.UI
{
    public class ShopItemSelectUI : ItemSelectUI
    {
        
        public TMPro.TMP_Text priceText;

       

        public Color RegularPriceColor;
        public Color SalePriceColor;



        PlayerGoldController PlayerGoldController;
        private void Start()
        {
            image = GetComponent<Image>();
            PlayerGoldController = Player.instance.goldController;
        }

        public void SetPriceText(int price)
        {
            priceText.text = price.ToString() + " <sprite=0>";
            priceText.color = RegularPriceColor;
        }

        public void SetPriceText(int price, bool isSale)
        {
            if (isSale)
            {
                priceText.text = price.ToString() + " <sprite=0>";
                priceText.color = SalePriceColor;
            }
            else
            {
                SetPriceText(price);
            }
            
        }
    }
}