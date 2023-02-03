using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Game.GameDebug.Cheats
{
    public class CheatsInvoker : MonoBehaviour
    {
        public Cheat[] cheats;

        private void Start()
        {
            Cheat.CheatActivationData data = new Cheat.CheatActivationData(
                SaveLoad.SaveLoadController.Instance.cheatsData.SaveData,
                GameMap.MainGameLevelMapController.Instance,
                PlayerController.Player.instance);

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
                SaveLoad.SaveLoadController.Instance.cheatsData.SaveData,
                GameMap.MainGameLevelMapController.Instance,
                PlayerController.Player.instance);

            foreach (Cheat cheat in cheats)
            {
                cheat.Deactivate(data);
            }
        }
    }
}