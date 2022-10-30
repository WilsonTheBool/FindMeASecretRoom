using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.LevelGeneration
{
    public class Room
    {
        public Vector2Int position;

        public Room_Figure Figure;

        public RoomType type;

        public int distance;

        public Vector2Int parent;



        public Room(Room_Figure room_Figure, int distance)
        {
            this.Figure = room_Figure;
            this.distance = distance;
        }

        public Room()
        {
            position = Vector2Int.zero;
        }
    }
}