using Assets.Scripts.Game.PlayerController;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Game.GameDebug.Cheats
{
    [CreateAssetMenu(menuName = "Debug/Cheats/Inf_Gold")]
    public class Cheat_InfGold : Cheat
    {
        public override void Activate(CheatActivationData data)
        {
            data.player.goldController.gold = 99;
            data.player.goldController.GoldChanged.Invoke(data.player.goldController);
            data.player.goldController.BeforeGoldRemove.AddListener(BeforeSpend);
        }

        private void BeforeSpend(PlayerGoldController.GoldEventArgs args)
        {
            args.ammountChanged = 0;
        }

        public override bool CanActivate(CheatActivationData data)
        {
            return data.cheatsData.infGold;
        }

        public override void Deactivate(CheatActivationData data)
        {
            data.player.goldController.BeforeGoldRemove.RemoveListener(BeforeSpend);
        }
    }
}