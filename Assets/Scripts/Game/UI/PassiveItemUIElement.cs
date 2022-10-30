using Assets.Scripts.Game.Items;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Game.UI
{
    public class PassiveItemUIElement : MonoBehaviour
    {
        public Item item;

        public Image image;

        public void SetUp(Item item)
        {
            this.item = item;
            image.sprite = item.Sprite;
        }
        
    }
}