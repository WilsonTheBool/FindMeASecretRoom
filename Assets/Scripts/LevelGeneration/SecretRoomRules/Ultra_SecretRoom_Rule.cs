using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.LevelGeneration.SecretRoomRules
{
    [CreateAssetMenu(menuName = "LevelGeneration/Rule/Ultra secret_Room")]
    public class Ultra_SecretRoom_Rule : SecretRoomGenerationRule
    {
        public RoomType SecretRoomType;

        public int searchRange;

        public override bool CanTryGenerate(LevelMap map, LevelGeneratorParams data, int countToGenerate)
        {
            return true;
        }


        public override bool GenerateRooms(LevelMap map, LevelGeneratorParams data, int countToGenerate)
        {
            RuleCostMap costMap = new RuleCostMap();

            List<Room> specialRooms = map.rooms.FindAll((room) => room.type != null && room.type.isSecretRoom);

            foreach (Room room in map.rooms)
            {
                foreach (Vector2Int exit in room.Figure.BlockedExits)
                {
                    Vector2Int key = exit + room.position;

                    if (map.IsInRange(key) && map.GetRoom(key) == null)
                        costMap.AddValue(key, -100);
                }

                foreach (Vector2Int exit in room.Figure.RoomExits)
                {
                    Vector2Int key = exit + room.position;


                    if (map.IsInRange(key) && map.GetRoom(key) == null)
                        if (!costMap.HasKey(key))
                            costMap.AddValue(key, IsPosIntercetcsSpecial(key, specialRooms, TileFigures.GetSquare_Filled(2, key)));


                }


            }

            costMap.ReSort();

            for (int i = 0; i < countToGenerate; i++)
            {
                if (!costMap.CanGetMax())
                {
                    return false;
                }

                Room secret = new Room(startRoom, 0)
                {
                    type = SecretRoomType
                };

                Vector2Int max = costMap.GetMaxKey_Randomized();

                if (map.IsInRange(max))
                {
                    map.PlaceRoom(secret, max, max);
                }
            }

            return true;
        }

        private int IsPosIntercetcsSpecial(Vector2Int p, List<Room> sp, Vector2Int[] square)
        {
            int reward = 0;

            List<Vector2Int> sq = new List<Vector2Int>(square);

            foreach (Room room in sp)
            {
                if (sq.Contains(room.position))
                {
                    reward++;
                }
            
            }

            return reward;
        }
    }
}