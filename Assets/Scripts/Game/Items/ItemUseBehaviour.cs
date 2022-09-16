using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Game.Items
{
    public abstract class ItemUseBehaviour : MonoBehaviour
    {
        public abstract bool CanUse(Item.ItemInternalEventArgs args);

    }
}