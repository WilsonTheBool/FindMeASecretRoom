using System;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Game.PlayerController;

namespace Assets.Scripts.Game.Items
{
    public class ActiveItem: MonoBehaviour
    {
        public Item item;

        public bool TryActivate(ActiveItemEventArgs args)
        {
            if (CanActivate(args))
            {
                Activate(args);

                return true;
            }
            else
            {
                return false;
            }
        }

        protected virtual bool CanActivate(ActiveItemEventArgs args)
        {
            return false;
        }

        protected virtual void Activate(ActiveItemEventArgs args)
        {

        }

        public class ActiveItemEventArgs
        {
            public Player player;

            public PlayerItemsController PlayerItemsController;
        }
    }
}
