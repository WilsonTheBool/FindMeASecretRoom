using Assets.Scripts.Game.Items.ItemPools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Assets.Scripts.Game.Items;
using Assets.Scripts.InputManager;
using UnityEngine.UI;
using Assets.Scripts.Game.PlayerController;

namespace Assets.Scripts.Game.UI
{
    public class TresureRoomUIController : MonoBehaviour
    {
        [HideInInspector]
        public List<ItemSelectUI> itemSelectUIs = new List<ItemSelectUI>();

        public ItemSelectUI prefab;

        public TMPro.TMP_Text itemDescription;
        public TMPro.TMP_Text itemName;
        public TMPro.TMP_Text activeText;
        public TMPro.TMP_Text onItemOverride;

        public ItemSelectUI.ItemSelecEvent ItemSelected;

        public CanvasGroup CanvasGroup;

        public InputListener InputListener;

        public Transform itemsHolder;

        public Button skipButton;

        private void Start()
        {
            skipButton.onClick.AddListener(RequestSkip);
            CloseWIndow();
        }

        public UnityEvent SkipRequested;

        public void RequestSkip()
        {
            SkipRequested.Invoke();
        }

        public void CreateWindow(Item[] items)
        {
            SetText(null);
            InputListener.enabled = true;

            foreach (Item item in items)
            {
                AddNreItemUI(item);
            }

            CanvasGroup.alpha = 1;
            CanvasGroup.interactable = true;
            CanvasGroup.blocksRaycasts = true;
            skipButton.enabled = true;
        }

        public void CloseWIndow()
        {
            while(itemSelectUIs.Count > 0)
            {
                RemoveItemUI(itemSelectUIs[0]);
            }

            CanvasGroup.alpha = 0;
            CanvasGroup.interactable = false;
            CanvasGroup.blocksRaycasts = false;

            InputListener.enabled = false;

            StopAllCoroutines();
        }
        
        private void AddNreItemUI(Item item)
        {
            var ui = Instantiate(prefab, itemsHolder.transform);
            itemSelectUIs.Add(ui);
            ui.SetUp(item);
            ui.Selected.AddListener(OnItemSelected);
            ui.PointerEnter.AddListener(OnItemEnter);
            ui.PointerExit.AddListener(OnItemExit);
        }

        private void RemoveItemUI(ItemSelectUI item)
        {
            int index = itemSelectUIs.IndexOf(item);

            if(index != -1)
            {
                itemSelectUIs[index].PointerEnter.RemoveAllListeners();
                itemSelectUIs[index].PointerExit.RemoveAllListeners();
                itemSelectUIs[index].Selected.RemoveAllListeners();

                Destroy(itemSelectUIs[index].gameObject);
                itemSelectUIs.RemoveAt(index);
            }
        }

        private void SetText(Item item)
        {
            if(item == null)
            {
                itemName.text = "";
                itemDescription.text = "";
                activeText.text = "";
                onItemOverride.text = "";
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

                PlayerItemsController controller = Player.instance.itemsController;
                if (item.isUseItem && controller.ActiveItems.Count == controller.maxACtiveItemsCount)
                {
                    onItemOverride.text = "Will remove " + controller.ActiveItems[1].Name;
                }
                else
                {
                    onItemOverride.text = "";
                }

            }
        }

        public void StartFadeAnimation(ItemSelectUI selected)
        {
            int index = itemSelectUIs.IndexOf(selected);

            StartCoroutine(ItemSelectAnim(index));
        }

        [SerializeField]
        private float fadeOutSpeed;

        private IEnumerator ItemSelectAnim(int selectedItemIndex)
        {
            foreach(ItemSelectUI item in itemSelectUIs)
            {
                item.CanvasGroup.interactable = false;
                item.CanvasGroup.blocksRaycasts = false;
            }

            if (itemSelectUIs.Count == 0 || fadeOutSpeed <= 0)
            {

            }
            else
            {


                //Fadeout
                for (int i = 0; i < itemSelectUIs.Count; i++)
                {
                    if (i != selectedItemIndex)
                    {
                        itemSelectUIs[i].CanvasGroup.alpha = 0;

                    }

                }


            }

            

            yield return null;
        }

        void OnItemEnter(ItemSelectUI item)
        {
            SetText(item.item);
        }

        void OnItemExit(ItemSelectUI item)
        {
            SetText(null);
        }

        void OnItemSelected(ItemSelectUI item)
        {
            ItemSelected.Invoke(item);
            skipButton.enabled = false;
        }
    }
}