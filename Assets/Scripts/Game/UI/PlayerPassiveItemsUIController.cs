using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Assets.Scripts.Game.PlayerController;
using Assets.Scripts.Game.Items;

namespace Assets.Scripts.Game.UI
{
    public class PlayerPassiveItemsUIController: MonoBehaviour
    {
        public List<PassiveItemUIElement> items;

        [SerializeField]
        private PassiveItemUIElement prefab;

        [HideInInspector]
        public PlayerItemsController PlayerItemsController;

        [SerializeField]
        private Material highlight;

        [SerializeField]
        private Material defaultMat;

        private void Start()
        {
            PlayerItemsController = Player.instance.itemsController;
            PlayerItemsController.ItemAdded.AddListener(OnItemAdd);
            PlayerItemsController.ItemRemoved.AddListener(OnItemRemove);

            SetUp();
        }

        private void SetUp()
        {
            foreach(Item item in PlayerItemsController.passiveItems)
            {
                OnItemAdd(item);
            }
        }

        public PassiveItemUIElement FindElement(Item item)
        {
            return items.Find((ui) => ui.item.Name == item.Name);
        }

        public PassiveItemUIElement FindElement(string name)
        {
            return items.Find((ui) => ui.item.Name == name);
        }

        public void SetHighlight(Item item, bool isHighlight)
        {
            var ui = FindElement(item);

            if(ui != null)
            {
                if(isHighlight)
                    ui.image.material = highlight;
                else
                    ui.image.material = defaultMat;
            }
        }

        private float animHighlightTime = 1f;
        public void StartHIghlightAnim(Item item)
        {
            StartCoroutine(HIghlight_Co(item));
        }

        private IEnumerator HIghlight_Co(Item item)
        {
            var ui = FindElement(item);

            if (ui != null)
            {
                ui.image.material = highlight;
                yield return new WaitForSeconds(animHighlightTime);
                ui.image.material = defaultMat;
            }
        }

        private void OnItemAdd(Item item)
        {
            if (item.isUseItem)
            {
                return;
            }

            PassiveItemUIElement newElem = Instantiate(prefab, this.transform);
            newElem.SetUp(item);
            items.Add(newElem);
        }

        private void OnItemRemove(Item item)
        {
            if (item.isUseItem)
            {
                return;
            }

            PassiveItemUIElement found = FindElement(item);

            if(found != null)
            {
                items.Remove(found);
                Destroy(found.gameObject);
            }
        }
    }
}
