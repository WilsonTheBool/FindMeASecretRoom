using Assets.Scripts.SaveLoad;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Game.GameDebug.Cheats
{
    public class CheatsInvoker : MonoBehaviour
    {
        public Cheat[] cheats;

        CheatsSaveLoadDataHolder saveData;

        private void Start()
        {
            SaveLoadController.Instance.TryGetSaveLoadComponent<CheatsSaveLoadDataHolder>(out saveData);

            Cheat.CheatActivationData data = new Cheat.CheatActivationData(
                saveData.SaveData,
                GameMap.MainGameLevelMapController.Instance,
                PlayerController.Player.instance) ;

            foreach(Cheat cheat in cheats)
            {
                if (cheat.CanActivate(data))
                {
                    cheat.Activate(data);
                }
            }
        }

        private void OnDestroy()
        {
            Cheat.CheatActivationData data = new Cheat.CheatActivationData(
                saveData.SaveData,
                GameMap.MainGameLevelMapController.Instance,
                PlayerController.Player.instance);

            foreach (Cheat cheat in cheats)
            {
                cheat.Deactivate(data);
            }
        }
    }
}