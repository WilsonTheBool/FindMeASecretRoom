using Assets.Scripts.Challenges;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.SaveLoad
{
    public class SaveLoadController : MonoBehaviour
    {

        public static SaveLoadController Instance;

        public List<ISaveLoadComponent> saveLoadComponents;

        public bool TryGetSaveLoadComponent<T>(out T Component) where T: class, ISaveLoadComponent 
        {
            foreach(ISaveLoadComponent component in saveLoadComponents)
            {
                if(component is T)
                {
                    Component = component as T;
                    return true;
                }
            }

            Component = null;
            return false;
        }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);

                SetUp();
            }
        }

        void SetUp()
        {
            saveLoadComponents = new List<ISaveLoadComponent>(GetComponentsInChildren<ISaveLoadComponent>());

            //All components setUp()
            foreach (ISaveLoadComponent component in saveLoadComponents)
            {
                component.SetUp(this);
            }

            //All components Load_Savedata() from disc
            foreach (ISaveLoadComponent component in saveLoadComponents)
            {
                component.Load_SaveData(this);
            }

            saveLoadComponents.Sort(new SaveLoadComponentComparer());


            foreach (ISaveLoadComponent component in saveLoadComponents)
            {
                component.OnAwake(this);
            }

            foreach (ISaveLoadComponent component in saveLoadComponents)
            {
                component.OnAfterAwake(this);
            }
        }

        private void Start()
        {
            foreach (ISaveLoadComponent component in saveLoadComponents)
            {
                component.OnStart(this);
            }
        }

        public T LoadObject<T>(string name)
        {
            return JsonUtility.FromJson<T>(PlayerPrefs.GetString(name));
        }
        
        public void SaveObject<T>(T obj, string name)
        {
            PlayerPrefs.SetString(name, JsonUtility.ToJson(obj));
            PlayerPrefs.Save();
        }

        class SaveLoadComponentComparer : IComparer<ISaveLoadComponent>
        {
            public int Compare(ISaveLoadComponent x, ISaveLoadComponent y)
            {
                return x.GetOrder().CompareTo(y.GetOrder());
            }
        }
    }
}