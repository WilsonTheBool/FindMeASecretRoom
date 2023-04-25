using Assets.Scripts.Game.PlayerController;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

namespace Assets.Scripts.Game.UI
{
    public class PlayerActiveItemUI_Mimic : MonoBehaviour
    {
        public int itemIndex;

        public Image image;

        private void OnEnable()
        {
            if(Player.instance != null)
            {
                var controller = Player.instance.itemsController;

                controller.ItemAdded.AddListener(OnItemAdd);
            }

            
        }

        private void OnItemAdd(Items.Item item)
        {
            if (item.isUseItem)
            {
                UpdateImage();
            }
        }

        private void OnDisable()
        {
            if (Player.instance != null)
            {
                var controller = Player.instance.itemsController;

                controller.ItemAdded.RemoveListener(OnItemAdd);
            }
        }

        private void UpdateImage()
        {
            if (Player.instance.itemsController.ActiveItems.Count > itemIndex)
            {
                image.sprite = Player.instance.itemsController.ActiveItems[itemIndex].Sprite;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }


    }
}