using System.Collections;
using UnityEngine;

namespace Assets.Scripts.SaveLoad
{
    public class OptionsSaveLoadDataHolder : MonoBehaviour
    {
        private const string optionsName = "Options";

        public GameOptionSaveData SaveData { get; private set; }
        private SaveLoadController controller;



        private void LoadSaveData()
        {
            SaveData = controller.LoadObject<GameOptionSaveData>(optionsName);

            SaveData ??= new GameOptionSaveData();
        }

        public void SaveOptions()
        {
            controller.SaveObject<GameOptionSaveData>(SaveData, optionsName);
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
}