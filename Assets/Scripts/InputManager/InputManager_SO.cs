using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace Assets.Scripts.InputManager
{
    [CreateAssetMenu(menuName = "Input/InputManager_SO")]
    public class InputManager_SO : ScriptableObject
    {
        [HideInInspector]
        public InputReader InputReader;

        private Dictionary<int, List<InputListener>> InputGroups = new Dictionary<int, List<InputListener>>();


        [HideInInspector]
        public UnityEvent InputGroupChanged;

        public void AddInputListener(InputListener inputListener)
        {
            if (InputGroups.ContainsKey(inputListener.InputGroup))
            {
                if(!InputGroups[inputListener.InputGroup].Contains(inputListener))
                InputGroups[inputListener.InputGroup].Add(inputListener);
            }
            else
            {
                InputGroups.Add(inputListener.InputGroup, new List<InputListener>());
                InputGroups[inputListener.InputGroup].Add(inputListener);
            }

            InputGroupChanged.Invoke();
        }

        public void RemoveInputListener(InputListener inputListener)
        {
            if (InputGroups.ContainsKey(inputListener.InputGroup))
            {
                InputGroups[inputListener.InputGroup].Remove(inputListener);

                if(InputGroups[inputListener.InputGroup].Count == 0)
                {
                    InputGroups.Remove(inputListener.InputGroup);
                }

                InputGroupChanged.Invoke();
            }

        }

        public InputListener[] GetCurentGroup()
        {
           List<int> keys = new List<int>(InputGroups.Keys);

            List<InputListener> result = new List<InputListener>();
            
            int curentMaxKey = 0;
            
            keys.Sort();

            bool fl = false;

            do
            {
                if(keys.Count <= 0)
                {
                    break;
                }

                curentMaxKey = keys[keys.Count - 1];

                result.AddRange(InputGroups[curentMaxKey]);

                foreach(InputListener listener in InputGroups[curentMaxKey])
                {
                    if (listener.passInputDown)
                    {
                        fl = true;
                        keys.RemoveAt(keys.Count - 1);
                        break;
                    }
                }
            }
            while(fl);


            return result.ToArray();


        }

    }

   
}