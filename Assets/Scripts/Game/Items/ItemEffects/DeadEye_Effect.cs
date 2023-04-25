using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Game.Items.ItemEffects
{
    public class DeadEye_Effect : ItemEffect
    {
        public int comboReq = 5;
        public int goldIncrease = 3;

        public override void OnEffectAdd(Item.ItemInternalEventArgs args)
        {
            args.external.player.roomRewardController.OnReward.AddListener(OnReward);
        }

        private void OnReward(PlayerController.PlayerRoomRewardController.RewardEventArgs args)
        {
            if(args.curentCombo == comboReq)
            {
                args.ammountChanged += goldIncrease;
            }
        }

        public override void OnEffectRemove(Item.ItemInternalEventArgs args)
        {
            args.external.player.roomRewardController.OnReward.RemoveListener(OnReward);
        }
    }
}