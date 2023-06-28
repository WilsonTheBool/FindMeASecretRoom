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

        private CheatsSaveLoadDataHolder cheats;

        public void Awake()
        {
            
        }


        public void Start()
        {
            controller = SaveLoadController.Instance;
            controller.TryGetSaveLoadComponent(out cheats);

            infHp.isOn = cheats.SaveData.infHP;
            infHp.onValueChanged.AddListener(OnInfHpToggleChange);
            infGold.isOn = cheats.SaveData.infGold;
            infGold.onValueChanged.AddListener(OnInfGoldToggleChange);
            showRooms.isOn = cheats.SaveData.showRooms;
            showRooms.onValueChanged.AddListener(OnShowRoomsToggleChange);
        }

        private void OnInfHpToggleChange(bool value)
        {
            cheats.SaveData.infHP = value;
        }

        private void OnInfGoldToggleChange(bool value)
        {
            cheats.SaveData.infGold = value;
        }

        private void OnShowRoomsToggleChange(bool value)
        {
            cheats.SaveData.showRooms = value;
        }

        private void OnDisable()
        {
            cheats.Save_SaveData(controller);
        }
    }
}