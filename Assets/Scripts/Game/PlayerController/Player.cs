using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Game.PlayerController
{
    public class Player : MonoBehaviour
    {
        public static Player instance;

        public PlayerHPController playerHPController;

        public PlayerItemsController itemsController;

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }

            if(playerHPController == null)
            {
                playerHPController = GetComponentInChildren<PlayerHPController>();
            }

            if (itemsController == null)
            {
                itemsController = GetComponentInChildren<PlayerItemsController>();
            }
        }

    }
}