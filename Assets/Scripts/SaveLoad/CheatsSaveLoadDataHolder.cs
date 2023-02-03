using System.Collections;
using UnityEngine;

namespace Assets.Scripts.SaveLoad
{
    public class CheatsSaveLoadDataHolder : MonoBehaviour
    {
        private const string SaveDataName = "Cheats";
        public CheatsData SaveData;

        private SaveLoadController controller;



        private void LoadSaveData()
        {
            SaveData = controller.LoadObject<CheatsData>(SaveDataName);

            SaveData ??= new CheatsData();
        }

        public void SaveOptions()
        {
            controller.SaveObject<CheatsData>(SaveData, SaveDataName);
        }

        public void Awake()
        {
            controller = SaveLoadController.Instance;

            if (controller == null)
            {
                controller = GetComponentInParent<SaveLoadController>();
            }

            LoadSaveData();
        }

    }

    [System.Serializable]
    public class CheatsData
    {

        public bool infHP;

        public bool infGold;

        public bool showRooms;

    }
}