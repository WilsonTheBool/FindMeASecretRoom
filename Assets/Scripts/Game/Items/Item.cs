using UnityEditor;
using UnityEngine;
using Assets.Scripts.Game.PlayerController;

namespace Assets.Scripts.Game.Items
{
    public class Item : MonoBehaviour
    {
        public ItemData itemData;

        public virtual void OnItemAdd(ItemAddArgs args)
        {

        }

        public virtual void OnItemRemove(ItemAddArgs args) 
        {
            
        }

        public class ItemAddArgs
        {
            public Player player;

            public PlayerItemsController playerItemsController;

            //etc...
        }
    }
}