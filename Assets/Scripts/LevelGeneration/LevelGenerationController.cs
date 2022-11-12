using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace Assets.Scripts.LevelGeneration
{
    public class LevelGenerationController : MonoBehaviour
    {

        public LevelGenerator levelGenerator;

        public LevelGeneratorParams LevelGeneratorParams;

        public bool hideSecretRooms;

       
        //public Room_GM roomPrefab;

        private void Awake()
        {
            
        }

        LevelMap levelMap;

        public LevelMap Generate()
        {
            int loopCount = 0;

            do
            {
                loopCount++;
                levelMap = new LevelMap();
                Debug.Log("Generation End");

                if (loopCount > 30)
                {
                    Debug.Log("Main Loop fail");
                    return null;
                }
            }
            while ((!levelGenerator.GenerateLevel(levelMap, LevelGeneratorParams)));

            return levelMap;
        }


       

        public void SetHideSecretRooms(bool value)
        {
            hideSecretRooms = value;
        }
    }
}