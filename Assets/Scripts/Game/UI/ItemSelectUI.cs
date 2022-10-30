using Assets.Scripts.Game.Items;
using Assets.Scripts.Game.PlayerController;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.Game.UI
{
    public class ItemSelectUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public Image itemImage;

        public Item item;
        public TMPro.TMP_Text itemName;
        public string ItemSavedName;
        public CanvasGroup CanvasGroup;

        public Sprite RegularSprite;
        public Sprite SelectedSprite;

        protected Image image;

        private void Start()
        {

            image = GetComponent<Image>();
        }
        public void SetUp(Item item)
        {
            this.item = item;
            itemImage.sprite = item.Sprite;
            itemName.text = item.Name;
            ItemSavedName = item.Name;
        }

        public void SetUp(Sprite sprite, string Name)
        {
            itemImage.sprite = sprite;
            itemName.text = Name;
            ItemSavedName = Name;
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            Selected.Invoke(this);
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            PointerEnter.Invoke(this);
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            PointerExit.Invoke(this);
        }

        public ItemSelecEvent Selected;
        public ItemSelecEvent PointerEnter;
        public ItemSelecEvent PointerExit;

        [System.Serializable]
        public class ItemSelecEvent: UnityEvent<ItemSelectUI>
        {

        }

        public void SetSprite_Regular()
        {
            image.sprite = RegularSprite;
        }

        public void SetSprite_Selected()
        {
            image.sprite = SelectedSprite;
        }
    }
}