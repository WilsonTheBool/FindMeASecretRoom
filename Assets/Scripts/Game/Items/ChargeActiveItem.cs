using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game.Items
{
    public class ChargeActiveItem: ActiveItem
    {
        [SerializeField]
        int maxCharge;

        public int curentCharge;



        protected override bool CanActivate(ActiveItemEventArgs args)
        {
            return curentCharge >= 1;
        }

        public void AddCharge(int num)
        {
            curentCharge += num;
        }
    }
}
