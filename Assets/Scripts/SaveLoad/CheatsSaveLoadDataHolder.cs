using System.Collections;
using UnityEngine;

namespace Assets.Scripts.SaveLoad
{
    public class CheatsSaveLoadDataHolder : SaveLoadComponent<CheatsData>
    {
        private const string SaveDataName = "Cheats";

        public override void SetUp(SaveLoadController controller)
        {
            saveName = SaveDataName;
        }
    }

    [System.Serializable]
    public class CheatsData: SaveData
    {

        public bool infHP;

        public bool infGold;

        public bool showRooms;

    }
}