using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.LevelGeneration.SecretRoomRules
{
    /// <summary>
    /// hermit room - switch for super secret. 
    /// is furthest from start and connected to least rooms
    /// </summary>
    [CreateAssetMenu(menuName = "LevelGeneration/Rule/Super secert")]
    public class SuperSecretRoomRule : SecretRoomGenerationRule
    {

        public RoomType SecretRoomType;



        public override bool GenerateRooms(LevelMap map, LevelGeneratorParams data, int countToGenerate)
        {
            RuleCostMap costMap = new RuleCostMap();

            foreach (Room room in map.rooms)
            {
                float cost = 10;

                if (room.type != null)
                {

                    cost = -100;

                }

                foreach (Vector2Int exit in room.Figure.RoomExits)
                {
                    Vector2Int key = exit + room.position;



                    if (map.IsInRange(key) && map.GetRoom(key) == null)
                    {
                        int dis = CalculateDistance(map, room, exit) + room.distance;

                        

                        if (!costMap.HasKey(key))
                            costMap.AddValue(key, cost + dis);
                        else
                            costMap.AddValue(key, -1000);


                    }




                }

                foreach (Vector2Int exit in room.Figure.BlockedExits)
                {
                    Vector2Int key = exit + room.position;


                    if (map.IsInRange(key) && map.GetRoom(key) == null)
                            costMap.AddValue(key, -1000);
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

                Debug.Log("Room:" + max + "Distance = " + (costMap.GetValue(max) - 10).ToString());
            }

            return true;
        }

        private int CalculateDistance(LevelMap levelMap, Room owner, Vector2Int exit_2_pos)
        {
            Vector2Int exit_1_pos = new Vector2Int(-1000,-1000);
            
            Room parent = levelMap.GetRoom(owner.parent);

            //if room does not have a parent we take any exit connected to its origin point;
            if (parent == null)
            {
                var e = owner.Figure.GetRoomExitsOfTile(new Vector2Int(0, 0));

                if (e.Length > 0)
                {
                    exit_1_pos = e[0];
                }
                else
                {
                    throw new System.Exception("cant find rooms connected to origin point");
                }
            }
            else
            {
                if (parent.Figure.isLarge)
                {
                    bool exitfound = false;
                    Vector2Int offset =  owner.position - parent.position;
                    foreach(Vector2Int exit in owner.Figure.RoomExits)
                    {
                        if(parent.Figure.IsRoomContainsPos(exit + offset))
                        {
                             exit_1_pos = exit;
                            exitfound = true;
                        }
                    }
                    
                    
                    if(!exitfound)
                    throw new System.Exception("cant find connecting rooms (2 large rooms)");
                }
                else
                {
                    exit_1_pos = parent.position - owner.position;

                }

            }



            //Debug.Log("Exit_1 = " + exit_1_pos.ToString() + "// Exit_2 = " + exit_2_pos.ToString());
            int distance = owner.Figure.GetDistance_FromLocal(exit_1_pos, exit_2_pos);

            if (distance == -1)
            {
                throw new System.Exception("Cant find exits");
            }

            

            return distance;
        }
    }
}