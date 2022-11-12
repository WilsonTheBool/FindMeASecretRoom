using Assets.Scripts.Game.GameMap;
using Assets.Scripts.Game.PlayerController;
using Assets.Scripts.Game.Pregression;
using Assets.Scripts.LevelGeneration;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Tutorial
{
    public class MainPracticeController:MonoBehaviour
    {
        public LevelData[] levelDatas;

        public RoomType[] typeToHide;

        public MainGameLevelMapController MainGameLevelMapController;

        private int savedId = -1;

        public bool renderEnded = true;

        private void Awake()
        {
            if(MainGameLevelMapController == null)
            {
                MainGameLevelMapController = FindObjectOfType<MainGameLevelMapController>();
            }
        }

        private void Start()
        {
            MainGameLevelMapController.onVictory.AddListener(LoadSavedLevel);
            MainGameLevelMapController.LevelMapRenderer.onRenderEnded.AddListener(() => renderEnded = true);
            MainGameLevelMapController.LevelMapRenderer.onRenderEnded.AddListener(ShowSecretRooms);
            MainGameLevelMapController.LevelMapRenderer.onRenderStarted.AddListener(() => renderEnded = false);

            Player.instance.playerHPController.afterTakeDamage.AddListener((PlayerHPController.HpEventArgs args) => Player.instance.playerHPController.HealToFullRedHP());

            MainGameLevelMapController.LevelGenerationController.LevelGeneratorParams = null;
            MainGameLevelMapController.SetUpLevel();
        }



        public void LoadLevel(int levelId)
        {
            if (!renderEnded)
            {
                return;
            }

            savedId = levelId;

            LoadSavedLevel();
        }

        public void LoadSavedLevel()
        {
            if (renderEnded)
                if (savedId >= 0 && savedId < typeToHide.Length)
                {
                    {
                        MainGameLevelMapController.LevelMapRenderer.ClearAll();

                        MainGameLevelMapController.LevelGenerationController.LevelGeneratorParams = levelDatas[savedId].generatorParams;
                        MainGameLevelMapController.SetUpLevel();
                        MainGameLevelMapController.LevelMapRenderer.StartRenderMap(MainGameLevelMapController.LevelMap);
                        MainGameLevelMapController.levelStarted.Invoke();
                    }
                }
        }

        private void ShowSecretRooms()
        {
            if (renderEnded)
                if (savedId >= 0 && savedId < typeToHide.Length)
                {
                    foreach (Room room in MainGameLevelMapController.LevelMap.rooms)
                    {
                        if (room.type != null && room.type.isSecretRoom && room.type != typeToHide[savedId])
                        {
                            MainGameLevelMapController.GameMapRoomUnlockController.TryUnlockRoom(room);
                            MainGameLevelMapController.LevelMapRenderer.RenderRoom_Unsafe(room);
                        }
                    }
                }
        }

        public void ShowHiddenRooms()
        {
            if (savedId >= 0 && savedId < typeToHide.Length)
            {
                foreach (Room room in MainGameLevelMapController.LevelMap.rooms)
                {
                    if (room.type != null && room.type == typeToHide[savedId])
                    {
                        MainGameLevelMapController.LevelMapRenderer.RenderRoom_Unsafe(room);
                    }
                }
            }

        }
    }
}
