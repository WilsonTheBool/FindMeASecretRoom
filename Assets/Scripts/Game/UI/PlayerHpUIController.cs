using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Game.PlayerController;

namespace Assets.Scripts.Game.UI
{
    public class PlayerHpUIController : MonoBehaviour
    {
        PlayerHPController PlayerHPController;

        [SerializeField]
        Image[] hpIcons;

        private void Start()
        {
            PlayerHPController = Player.instance.playerHPController;
            PlayerHPController.onAnyHpChanged.AddListener(UpdateHpUI);

            UpdateHpUI(null);
        }

        private void UpdateHpUI(PlayerHPController.HpEventArgs args)
        {
            foreach(Image hp in hpIcons)
            {
                hp.gameObject.SetActive(false);
            }

            var list = PlayerHPController.hpObjects;

            for (int i = 0; i < list.Count; i++)
            {
                if(i < hpIcons.Length)
                {
                    hpIcons[i].gameObject.SetActive(true);
                    hpIcons[i].sprite = list[i].sprite;
                }
            }
        }
    }
}