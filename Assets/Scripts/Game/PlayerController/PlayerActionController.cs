using System.Collections;
using Assets.Scripts.Game.Items;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Game.PlayerController
{
    public class PlayerActionController : MonoBehaviour
    {
        public static UnityEvent ActionPerformed;

        private void Awake()
        {
            if(ActionPerformed == null)
            {
                ActionPerformed = new UnityEvent();
            }
        }

        private void Start()
        {
            Player.instance.itemsController.OnAfterItemUsed.AddListener(OnItemUse);
        }

        public void OnItemUse(Item item, Item.ItemExternalEventArgs args)
        {
            ActionPerformed?.Invoke();
        }
    }
}