using System.Collections;
using Assets.Scripts.Game.PlayerController;
using Assets.Scripts.Game.Items;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Game.UI
{
    public class PlayerActiveItemsUIController : MonoBehaviour
    {
        PlayerItemsController PlayerItemsController;

        public Image[] items;

        public Image itemCharge;

        public Sprite noCharge;
        public Sprite fullCharge;
        public Sprite doubleCharge;

        public Material active;

        public TMPro.TMP_Text swapTip;
        public TMPro.TMP_Text altFireTip;

        private void Start()
        {
            PlayerItemsController = Player.instance.itemsController;

            PlayerItemsController.ActiveItemSwitched.AddListener(UpdateUI);
            PlayerItemsController.ActiveItemUpdated.AddListener(UpdateUI);
            PlayerItemsController.ItemAdded.AddListener(UpdateUI);
            PlayerItemsController.ItemRemoved.AddListener(UpdateUI);

            PlayerItemsController.OnItemUsed.AddListener(OnItemUsed);

            UpdateUI(null);
        }

        private void OnItemUsed(Item item, Item.ItemExternalEventArgs args)
        {
            if (item.isChargeItem)
            {
                UpdateUI(item);
            }
        }

        private void UpdateUI(Item item)
        {
            for(int i = 0; i < items.Length; i++)
            {
                if(i < PlayerItemsController.CurentActiveItems)
                {
                    items[i].transform.parent.gameObject.SetActive(true);
                }
                else
                {
                    items[i].transform.parent.gameObject.SetActive(false);
                }
            }

            for (int i = 0; i < PlayerItemsController.CurentActiveItems; i++)
            {
                int count = i + PlayerItemsController.selectedActiveItem;

                if (count >= PlayerItemsController.CurentActiveItems)
                {
                    count -= PlayerItemsController.CurentActiveItems;
                }

                items[i].sprite = PlayerItemsController.ActiveItems[count].Sprite;

            }

            Item selected = PlayerItemsController.ActiveItems[PlayerItemsController.selectedActiveItem];

            if (PlayerItemsController.CurentActiveItems > 0 && selected.isChargeItem)
            {
                itemCharge.gameObject.SetActive(true);

                if(selected.curentCharge == 0)
                {
                    itemCharge.sprite = noCharge;
                }
                else
                {
                    if (selected.curentCharge == 1)
                    {
                        itemCharge.sprite = fullCharge;
                    }
                    else
                    {
                        if (selected.curentCharge >= 2)
                        {
                            itemCharge.sprite = doubleCharge;
                        }
                    }
                }

            }
            else
            {
                itemCharge.gameObject.SetActive(false);
            }

            if (PlayerItemsController.CurentActiveItems > 0 && selected.CanUseItem_AtAll())
            {
                items[0].material = active;
            }
            else
            {
                items[0].material = null;
            }

            if(PlayerItemsController.CurentActiveItems > 1)
            {
                swapTip?.gameObject.SetActive(true);
            }
            else
            {
                swapTip?.gameObject.SetActive(false);
            }

            if (selected.hasAltMode)
            {
                altFireTip?.gameObject.SetActive(true);
            }
            else
            {
                altFireTip?.gameObject.SetActive(false);
            }
        }
    }
}