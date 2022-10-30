using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
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

        private void Start()
        {
            PlayerItemsController = Player.instance.itemsController;
            PlayerItemsController.ItemAdded.AddListener(OnItemAdd);
            PlayerItemsController.ItemRemoved.AddListener(OnItemRemove);
        }

        public PassiveItemUIElement FindElement(Item item)
        {
            return items.Find((ui) => ui.item.Name == item.Name);
        }

        public PassiveItemUIElement FindElement(string name)
        {
            return items.Find((ui) => ui.item.Name == name);
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
