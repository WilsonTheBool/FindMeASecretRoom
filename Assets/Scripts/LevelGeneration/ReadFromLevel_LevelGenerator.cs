using Assets.Scripts.Game.GameMap;
using Assets.Scripts.Tutorial;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.LevelGeneration
{
    [CreateAssetMenu(menuName = "LevelGeneration/Tutorial Generator")]
    public class ReadFromLevel_LevelGenerator : LevelGenerator
    {
        private MainGameLevelMapController MainGameLevelMapController;
        private MainTutorialController MainTutorialController;

        private Vector2Int startPos;
        public override bool GenerateLevel(LevelMap levelMap, LevelGeneratorParams data)
        {

            if(MainGameLevelMapController == null)
            {
                MainGameLevelMapController = FindObjectOfType<MainGameLevelMapController>();
            }
            if (MainTutorialController == null)
            {
                MainTutorialController = FindObjectOfType<MainTutorialController>();
            }


            GridMap gridMap = MainGameLevelMapController.grid;

            Room_GM_TypeHolder[] gms = MainTutorialController.GetCurentLevel();

            foreach (var room in gms)
            {

                Vector2Int pos = gridMap.WorldToCell(room.transform.position);

                if (room.parentDirection == new Vector2Int(0, 0))
                {
                    startPos = pos;
                }


                if (levelMap.IsInRange(pos))
                levelMap.PlaceRoom(new Room(){ Figure = room.Room_Figure, type = room.RoomType }, pos, pos + room.parentDirection);

                room.gameObject.SetActive(false);
            }



            return true;
        }

        public override Vector2Int[] GetStartRooms()
        {
            return new Vector2Int[] {startPos};
        }
    }
}