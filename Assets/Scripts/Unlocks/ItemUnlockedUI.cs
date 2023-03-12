using System.Collections;
using Assets.Scripts.Game.Items;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Unlocks
{
    public class ItemUnlockedUI : MonoBehaviour
    {

        public Image icon;

        public TMPro.TMP_Text itemName;

        public TMPro.TMP_Text description;

       public void SetUp(Item item)
        {
            icon.sprite = item.Sprite;
            itemName.text = item.Name;
            description.text = item.Description;
        }
    }
}