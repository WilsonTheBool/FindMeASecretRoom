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
        public Button button;

        private void Start()
        {
            PlayerTrinketController = Player.instance.trinketController;
            PlayerTrinketController.OnTrinketChanged.AddListener(UpdateUI);
            if(button == null)
            {
                button = GetComponent<Button>();
            }

            button.onClick.AddListener(() => PlayerTrinketController.UseTrinket(new Items.Item.ItemExternalEventArgs()
            {
                mainGameController = GameMap.MainGameLevelMapController.Instance,
                player = Player.instance
            }));
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