using Assets.Scripts.Game.PlayerController;
using UnityEditor;
using UnityEngine;
using static Assets.Scripts.Game.GameDebug.Cheats.Cheat;

namespace Assets.Scripts.Game.GameDebug.Cheats
{
    [CreateAssetMenu(menuName = "Debug/Cheats/Show_rooms")]
    public class Cheat_ShowRooms : Cheat
    {
        public override void Activate(CheatActivationData data)
        {
            data.main.LevelMapRenderer.renderSecretRoomsOnStart = true;
        }

        public override bool CanActivate(CheatActivationData data)
        {
            return data.cheatsData.showRooms;
        }

        public override void Deactivate(CheatActivationData data)
        {
            data.main.LevelMapRenderer.renderSecretRoomsOnStart = false;
        }
    }
}