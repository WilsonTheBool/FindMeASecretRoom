using Assets.Scripts.Game.Items;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Game.PlayerController
{
    public class PlayerTrinketController : MonoBehaviour
    {
        public Item trinket;

        public UnityEvent OnTrinketChanged;

        public void SetTrinket(Item item)
        {
            trinket = item;
            OnTrinketChanged.Invoke();
        }

        public void UseTrinket(Item.ItemExternalEventArgs args)
        {
            if(trinket == null)
            {
                return;
            }

            trinket.TryUseItem(args);
            
        }
    }
}