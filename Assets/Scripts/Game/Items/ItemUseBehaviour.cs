using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Game.Items
{
    public abstract class ItemUseBehaviour : MonoBehaviour
    {
        public abstract bool CanUse(Item.ItemInternalEventArgs args);

        public abstract void OnUse(Item.ItemInternalEventArgs args);

        public UnityEngine.Events.UnityEvent ItemUsed;

        public abstract void OnSelected(Item.ItemInternalEventArgs args);

        public abstract void OnDeselected(Item.ItemInternalEventArgs args);

        public abstract void OnAlternativeUse(Item.ItemInternalEventArgs args);

        public abstract void OnTilePosChnaged(Item.ItemInternalEventArgs args);
    }
}