using System.Collections;
using Assets.Scripts.Game.Items;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Unlocks
{
    public class ItemUnlockedUI : MonoBehaviour
    {
        public TMPro.TMP_Text title;

        public Image icon;

        public TMPro.TMP_Text itemName;

        public TMPro.TMP_Text description;

       public void SetUp(Item item)
        {
            icon.sprite = item.Sprite;
            itemName.text = item.Name;
            description.text = item.Description;
        }

        public void SetUp(string title, string itemName, string description, Sprite icon)
        {
            this.title.text = title;

            if(icon != null)
            {
               this. icon.sprite = icon;
            }
            else
            {
                this.icon.gameObject.SetActive(false);
            }
            
            this.itemName.text = itemName;
            this.description.text = description;
        }
    }
}