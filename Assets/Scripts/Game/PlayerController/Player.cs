using Assets.Scripts.Game.UI;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Game.PlayerController
{
    public class Player : MonoBehaviour
    {
        public static Player instance;

        public PlayerHPController playerHPController;

        public PlayerItemsController itemsController;

        public PlayerGoldController goldController;

        [HideInInspector]
        public GameUIController gameUIController;

        public PlayerRoomRewardController roomRewardController;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(instance);
                instance = this;
            }

            if (playerHPController == null)
            {
                playerHPController = GetComponentInChildren<PlayerHPController>();
            }

            if (itemsController == null)
            {
                itemsController = GetComponentInChildren<PlayerItemsController>();
            }

            if (roomRewardController == null)
            {
                roomRewardController = GetComponentInChildren<PlayerRoomRewardController>();
            }
        }

        private void Start()
        {
            if(gameUIController == null)
            {
                gameUIController = GameUIController.Instance;
            }
        }

    }
}