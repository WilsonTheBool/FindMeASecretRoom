﻿using System.Collections.Generic;
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

        public bool isLargeMap;

        public Vector2Int LargeMapSize;
        public Vector2Int LargeMapStart;
       
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
                

                if (true)
                {
                    levelMap = new LevelMap(LargeMapSize.x, LargeMapSize.y);
                    levelMap.StartRoomX = LargeMapStart.x;
                    levelMap.StartRoomY = LargeMapStart.y;
                    levelMap.LevelX = LargeMapSize.x;
                    levelMap.LevelY = LargeMapSize.y;
                    levelMap.startRooms = levelGenerator.GetStartRooms();
                }
               

                //Debug.Log("Generation End");

                if (loopCount > 50)
                {
                    Debug.Log("Main Loop fail");
                    return null;
                }
            }
            while ((!levelGenerator.GenerateLevel(levelMap, LevelGeneratorParams)));

            Debug.Log("Generation try count:" + loopCount + "/" + "50");

            return levelMap;
        }


       

        public void SetHideSecretRooms(bool value)
        {
            hideSecretRooms = value;
        }
    }
}