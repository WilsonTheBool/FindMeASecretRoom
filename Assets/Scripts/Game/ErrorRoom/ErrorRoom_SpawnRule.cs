using Assets.Scripts.LevelGeneration;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Game.ErrorRoom
{
    [CreateAssetMenu(menuName = "LevelGeneration/Rule/Error_Secret_Room")]
    public class ErrorRoom_SpawnRule : SecretRoomGenerationRule
    {

        public RoomType SecretRoomType;

        public RuleCostMap costMap;

        public ErrorRoomController prefab;

        public override bool CanTryGenerate(LevelMap map, LevelGeneratorParams data, int countToGenerate)
        {
            return true;
        }

        public override bool GenerateRooms(LevelMap map, LevelGeneratorParams data, int countToGenerate)
        {
            RuleCostMap costMap = new RuleCostMap();

            this.costMap = costMap;

            foreach (Room room in map.rooms)
            {
                float cost = 10;


                foreach (Vector2Int exit in room.Figure.RoomExits)
                {
                    Vector2Int key = exit + room.position;

                    if (map.IsInRange(key) && map.GetRoom(key) == null)
                    {
                        if (!costMap.HasKey(key) && room.type != null && !room.type.isSecretRoom)
                        {
                            costMap.AddValue(key, cost);
                        }
                        else
                            costMap.AddValue(key, -100);
                    }

                }

                    foreach (Vector2Int exit in room.Figure.BlockedExits)
                    {
                        Vector2Int key = exit + room.position;

                        if (map.IsInRange(key) && map.GetRoom(key) == null)
                            costMap.AddValue(key, -100);
                    }
            }

            costMap.ReSort();


            Vector2Int[] roomPositions = costMap.GetAllMax();

            Debug.Log("Error room positions count: " + roomPositions.Length);

            if(roomPositions.Length <= countToGenerate)
            {
                return false;
            }

            for (int i = 0; i < countToGenerate; i++)
            {
                Room secret = new Room(startRoom, 0)
                {
                    type = SecretRoomType
                };

                Vector2Int max = roomPositions[0];

                if (map.IsInRange(max))
                {
                    map.PlaceRoom(secret, max, max);
                }

                if(ErrorRoomController.Instance == null)
                {
                    Instantiate<ErrorRoomController>(prefab).AddErrorRoom(secret, roomPositions);
                    Debug.Log("Instatiating ErrorRoomController!!");
                }
                else
                {
                    ErrorRoomController.Instance.AddErrorRoom(secret, roomPositions);
                    Debug.Log("Geting static ErrorRoomController");
                }
                
            }

            return true;
        }
    }
}