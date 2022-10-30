using Assets.Scripts.Game.PlayerController;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Game.UI
{
    public class ComboCounterUI : MonoBehaviour
    {

        public TMPro.TMP_Text ComboText;

        private PlayerRoomRewardController PlayerRoomRewardController;

       

        private void Start()
        {
            PlayerRoomRewardController = Player.instance.roomRewardController;
            PlayerRoomRewardController.onComboChanged.AddListener(UpdateText);

            UpdateText();
        }

        private void UpdateText()
        {
            ComboText.text = PlayerRoomRewardController.curentCombo.ToString();
        }
    }
}