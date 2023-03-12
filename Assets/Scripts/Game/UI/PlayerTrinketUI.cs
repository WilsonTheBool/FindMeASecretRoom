using Assets.Scripts.Game.PlayerController;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Game.UI
{
    public class PlayerTrinketUI : MonoBehaviour
    {
        public PlayerTrinketController PlayerTrinketController;
        public Image item;
        public GameObject holder;

        private void Start()
        {
            PlayerTrinketController = Player.instance.trinketController;
            PlayerTrinketController.OnTrinketChanged.AddListener(UpdateUI);

            UpdateUI();
        }

        private void UpdateUI()
        {
            if(PlayerTrinketController.trinket == null)
            {
                holder.SetActive(false);
            }
            else
            {
                holder.SetActive(true);
                item.sprite = PlayerTrinketController.trinket.Sprite;

            }
        }
    }
}