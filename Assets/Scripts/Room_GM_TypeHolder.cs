using Assets.Scripts.LevelGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public  class Room_GM_TypeHolder: Room_GM
    {
        public RoomType RoomType;

        public Room_Figure Room_Figure;

        public Vector2Int parentDirection;

        private void Awake()
        {
            if(RoomType != null)
            {
                if (RoomType.overridesColor)
                {
                    baseRenderer.color = RoomType.colorOfBase;
                }

                iconRenderer.sprite = RoomType.icon;
            }
        }
    }
}
