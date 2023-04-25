using System.Collections;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Game.PlayerController
{
    public class PlayerGoldController : MonoBehaviour
    {
        public int gold;

        public int maxGold;

        public GoldEvent BeforeGoldAdd;
        public GoldEvent BeforeGoldRemove;
        public UnityEvent<PlayerGoldController> GoldChanged;

        public Sprite goldSprite;

        public void AddGold(int count)
        {
            GoldEventArgs args = new GoldEventArgs(count);

            BeforeGoldAdd.Invoke(args);

            gold += args.ammountChanged;

            if(gold > maxGold)
            {
                gold = maxGold;
            }

            GoldChanged.Invoke(this);
        }

        public void RemoveGold(int count)
        {
            GoldEventArgs args = new GoldEventArgs(count);

            BeforeGoldRemove.Invoke(args);

            gold -= args.ammountChanged;

            if (gold < 0)
            {
                gold = 0;
            }

            GoldChanged.Invoke(this);
        }

        public bool CanSpendGold(int ammount)
        {
            return gold >= ammount;
        }

        public class GoldEventArgs
        {
            public readonly int ammount;

            public int ammountChanged;

            public GoldEventArgs(int ammount)
            {
                this.ammount = ammount;
                this.ammountChanged = ammount;
            }
        }

        [System.Serializable]
        public class GoldEvent : UnityEvent<GoldEventArgs>
        {

        }
    }
}