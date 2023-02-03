using Assets.Scripts.Game.PlayerController;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Game.GameDebug.Cheats
{
    [CreateAssetMenu(menuName = "Debug/Cheats/Inf_HP")]
    public class Cheat_InfHP : Cheat
    {
        public override void Activate(CheatActivationData data)
        {
            data.player.playerHPController.beforeTakeDamage.AddListener(BeforeDamage);
        }

        private void BeforeDamage(PlayerHPController.HpEventArgs args)
        {
            args.change = 0;
        }

        public override bool CanActivate(CheatActivationData data)
        {
            return data.cheatsData.infHP;
        }

        public override void Deactivate(CheatActivationData data)
        {
            data.player.playerHPController.beforeTakeDamage.RemoveListener(BeforeDamage);
        }
    }
}