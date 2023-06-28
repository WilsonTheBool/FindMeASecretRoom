using System.Collections;
using UnityEngine;

namespace Assets.Scripts.SaveLoad
{
    public class OptionsSaveLoadDataHolder : SaveLoadComponent<GameOptionSaveData>
    {
        private const string optionsName = "Options";

        public override void SetUp(SaveLoadController controller)
        {
            saveName = optionsName;
        }
    }
}