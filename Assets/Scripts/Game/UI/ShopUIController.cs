using Assets.Scripts.Game.Items.ItemPools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Assets.Scripts.Game.Items;
using Assets.Scripts.InputManager;
using UnityEngine.UI;
using Assets.Scripts.Game.Pregression;
using Assets.Scripts.Game.PlayerController;

namespace Assets.Scripts.Game.UI
{
    public class ShopUIController : MonoBehaviour
    {

        public List<ShopItemSelectUI> itemSelectUIs = new List<ShopItemSelectUI>();

        [HideInInspector]
        public ShopItemSelectUI redHeartUi;

        [HideInInspector]
        public ShopItemSelectUI blueHeartUi;

        public ShopItemSelectUI prefab;
        public ShopItemSelectUI hp_prefab;

        public TMPro.TMP_Text ifCantBuy;
        public TMPro.TMP_Text itemDescription;
        public TMPro.TMP_Text onItemOverride;
        public TMPro.TMP_Text itemName;
        public TMPro.TMP_Text activeText;

        public ItemSelectUI.ItemSelecEvent ItemSelected;

        public CanvasGroup CanvasGroup;

        public InputListener InputListener;

        public Transform itemsHolder;

        public Transform healHolder;

        public Button skipButton;

        private ShopRoomController ShopRoomController;

        private void Start()
        {
            skipButton.onClick.AddListener(RequestSkip);
            CloseWIndow();
            //SetText(null);
        }

        public UnityEvent SkipRequested;
        public UnityEvent OnBuy;
        public UnityEvent OnCantBuy;

        public void RequestSkip()
        {
            SkipRequested.Invoke();
        }

        public void CreateWindow(ShopRoomController.PriceData[] items, ShopRoomController shopRoomController, bool spawnHp)
        {
            SetText(null);

            ShopRoomController = shopRoomController;

            

            if(spawnHp)
            AddHpObject(shopRoomController.redHp);

            if(spawnHp)
            AddHpObject(shopRoomController.blueHp);

            foreach (var item in items)
            {
                AddNreItemUI(item);
            }

            //CanvasGroup.alpha = 1;
            //CanvasGroup.interactable = true;
            //CanvasGroup.blocksRaycasts = true;
            //InputListener.enabled = true;

            gameObject.SetActive(true);

        }

        public void CloseWIndow()
        {
            while (itemSelectUIs.Count > 0)
            {
                Destroy(itemSelectUIs[0].gameObject);
                RemoveItemUI(itemSelectUIs[0]);
            }
            if(redHeartUi != null)
            {
                Destroy(redHeartUi.gameObject);
                redHeartUi = null;
            }

            if (blueHeartUi != null)
            {
                Destroy(blueHeartUi.gameObject);
                blueHeartUi = null;
            }
           

            //CanvasGroup.alpha = 0;
            //CanvasGroup.interactable = false;
            //CanvasGroup.blocksRaycasts = false;

            //InputListener.enabled = false;

            gameObject.SetActive(false);
        }

        private void AddNreItemUI(ShopRoomController.PriceData data)
        {
            var ui = Instantiate(prefab, itemsHolder.transform);
            itemSelectUIs.Add(ui);
            ui.SetUp(data.item);
            ui.SetPriceText(data.price, data.isSale);
            ui.Selected.AddListener(OnItemSelected);
            ui.PointerEnter.AddListener(OnItemEnter);
            ui.PointerExit.AddListener(OnItemExit);
        }

        private void AddHpObject(HpObject hp)
        {
            var ui = Instantiate(hp_prefab, healHolder.transform);

            if(hp == ShopRoomController.redHp)
            {
                redHeartUi = ui;
            }
            else
            {
                if (hp == ShopRoomController.blueHp)
                {
                    blueHeartUi = ui;
                }
            }
            ui.SetUp(hp.sprite, hp.Name);
            ui.SetPriceText(ShopRoomController.GetPrice(hp));
            ui.Selected.AddListener(OnItemSelected);
            ui.PointerEnter.AddListener(OnItemEnter);
            ui.PointerExit.AddListener(OnItemExit);
            ui.itemName.transform.parent.gameObject.SetActive(false);
        }

        private void RemoveItemUI(ShopItemSelectUI item)
        {
            int index = itemSelectUIs.IndexOf(item);

            if (index != -1)
            {
                Destroy(itemSelectUIs[index].gameObject);
                itemSelectUIs.RemoveAt(index);
            }
        }

        private void SetText(Item item)
        {
            if (item == null)
            {
                itemName.text = "";
                itemDescription.text = "";
                activeText.text = "";
                onItemOverride.text = "";
                ifCantBuy.gameObject.SetActive(false);

            }
            else
            {
                itemName.text = item.Name;
                itemDescription.text = item.Description;

                if (item.isUseItem)
                {
                    activeText.text = "Active item";
                }
                else
                {
                    activeText.text = "Passive item";
                }

                if (ShopRoomController.CanBuy(ShopRoomController.GetPrice(item)))
                {
                    ifCantBuy.gameObject.SetActive(false);
                }
                else
                {
                    ifCantBuy.gameObject.SetActive(true);
                }

                PlayerItemsController controller = Player.instance.itemsController;
                if(item.isUseItem && controller.ActiveItems.Count == controller.maxACtiveItemsCount)
                {
                    onItemOverride.text = "Will remove " + controller.ActiveItems[1].Name;
                }
                else
                {
                    onItemOverride.text = "";
                }

            }
        }

        void OnItemEnter(ItemSelectUI item)
        {
            SetText(item.item);
        }

        void OnItemExit(ItemSelectUI item)
        {
            SetText(null);
        }

        public void OnHealSelect(bool isRed)
        {

        }

        void OnItemSelected(ItemSelectUI item)
        {

            if(item.item == null)
            {


                if (item.ItemSavedName == ShopRoomController.redHp.Name)
                {

                    if (ShopRoomController.Request_BuyHeart_Red())
                    {
                        Destroy(redHeartUi.gameObject);
                        redHeartUi = null;
                        ItemSelected.Invoke(item);
                        OnBuy.Invoke();
                        return;
                    }
                }
                else
                {
                    if (item.ItemSavedName == ShopRoomController.blueHp.Name)
                    {

                        if (ShopRoomController.Request_BuyHeart_Blue())
                        {
                            Destroy(blueHeartUi.gameObject);
                            blueHeartUi = null;
                            ItemSelected.Invoke(item);
                            OnBuy.Invoke();
                            return;
                        }
                    }
                   
                }
            }
            else
            {
                if (ShopRoomController.Request_Buy(item.item))
                {
                    itemSelectUIs.Remove(item as ShopItemSelectUI);
                    Destroy(item.gameObject);
                    ItemSelected.Invoke(item);
                    OnBuy.Invoke();
                    return;
                }
            }



            OnCantBuy.Invoke();

        }
    }
}