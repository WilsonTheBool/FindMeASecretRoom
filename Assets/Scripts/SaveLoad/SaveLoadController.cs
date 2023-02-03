using System.Collections;
using UnityEngine;

namespace Assets.Scripts.SaveLoad
{
    public class SaveLoadController : MonoBehaviour
    {

        public static SaveLoadController Instance;

        [HideInInspector]
        public OptionsSaveLoadDataHolder optionsData;

        [HideInInspector]
        public CheatsSaveLoadDataHolder cheatsData;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
            }
            else
            {

                Instance = this;
                DontDestroyOnLoad(gameObject);

                if (optionsData == null)
                {
                    optionsData = GetComponentInChildren<OptionsSaveLoadDataHolder>();
                }

                if (cheatsData == null)
                {
                    cheatsData = GetComponentInChildren<CheatsSaveLoadDataHolder>();
                }
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
    }
}