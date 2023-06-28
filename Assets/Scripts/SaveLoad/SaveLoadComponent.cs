using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.XR;

namespace Assets.Scripts.SaveLoad
{
    public abstract class SaveLoadComponent<T>: MonoBehaviour, IComparable<SaveLoadComponent<T>>, ISaveLoadComponent where T: SaveData, new()
    {
        public T SaveData;

        [HideInInspector]
        public string saveName;

        [HideInInspector]
        public int awakeOrder;

        public abstract void SetUp(SaveLoadController controller);

        public virtual void OnAwake(SaveLoadController controller) { }

        public virtual void OnAfterAwake(SaveLoadController controller) { }

        public virtual void OnStart(SaveLoadController controller) { }

        public virtual void Load_SaveData(SaveLoadController controller)
        {
            SaveData = controller.LoadObject<T>(saveName);

            SaveData ??= new T();
        }

        public virtual void Save_SaveData(SaveLoadController controller)
        {
            controller.SaveObject<T>(SaveData, saveName);
        }

        public int CompareTo(SaveLoad.SaveLoadComponent<T> other)
        {
            return this.awakeOrder.CompareTo(other.awakeOrder);
        }

        public int GetOrder()
        {
            return awakeOrder;
        }
    }
    
    public class SaveData { }
}