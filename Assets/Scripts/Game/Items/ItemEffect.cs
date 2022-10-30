using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Game.Items
{
    public abstract class ItemEffect : MonoBehaviour
    {

        public abstract void OnEffectAdd(Item.ItemInternalEventArgs args);


        public abstract void OnEffectRemove(Item.ItemInternalEventArgs args);


        public UnityEngine.Events.UnityEvent OnEffectActivated;
    }
}