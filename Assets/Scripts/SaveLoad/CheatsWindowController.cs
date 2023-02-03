using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.SaveLoad
{
    public class CheatsWindowController : MonoBehaviour
    {
        public Toggle infHp;
        public Toggle infGold;
        public Toggle showRooms;

        private SaveLoadController controller;
        public void Awake()
        {
            
        }


        public void Start()
        {
            controller = SaveLoadController.Instance;

            infHp.isOn = controller.cheatsData.SaveData.infHP;
            infHp.onValueChanged.AddListener(OnInfHpToggleChange);
            infGold.isOn = controller.cheatsData.SaveData.infGold;
            infGold.onValueChanged.AddListener(OnInfGoldToggleChange);
            showRooms.isOn = controller.cheatsData.SaveData.showRooms;
            showRooms.onValueChanged.AddListener(OnShowRoomsToggleChange);
        }

        private void OnInfHpToggleChange(bool value)
        {
            controller.cheatsData.SaveData.infHP = value;
        }

        private void OnInfGoldToggleChange(bool value)
        {
            controller.cheatsData.SaveData.infGold = value;
        }

        private void OnShowRoomsToggleChange(bool value)
        {
            controller.cheatsData.SaveData.showRooms = value;
        }

        private void OnDisable()
        {
            controller.cheatsData.SaveOptions();
        }
    }
}