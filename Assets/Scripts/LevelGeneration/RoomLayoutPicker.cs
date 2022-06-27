using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.LevelGeneration
{
    [CreateAssetMenu(menuName = "LevelGeneration/LayoutPicker")]
    public class RoomLayoutPicker : ScriptableObject
    {
        public LayoutHolder[] LayoutHolders;

        int sum = -1;

        public LayoutHolder[] TreasureRoomLayouts;
        public LayoutHolder[] BossRoomLayouts;

        public LayoutHolder emptyLayout;

        public Room_GM GetRoomObjectFromLayout(Room_Figure room_Figure)
        {
            foreach(LayoutHolder layoutHolder in LayoutHolders)
            {
                if(layoutHolder.Room_Figure == room_Figure)
                {
                    return layoutHolder.Room;
                }
            }

            return LayoutHolders[0].Room;
        }

        public Room_GM GetRoomObjectFromLayout(Room_Figure room_Figure, RoomType type)
        {
            if(type == null)
            {
                return GetRoomObjectFromLayout(room_Figure);
            }

            if(type.id == 0)
            foreach (LayoutHolder layoutHolder in BossRoomLayouts)
            {
                if (layoutHolder.Room_Figure == room_Figure)
                {
                    return layoutHolder.Room;
                }
            }

            if (type.id == 1)
                foreach (LayoutHolder layoutHolder in TreasureRoomLayouts)
                {
                    if (layoutHolder.Room_Figure == room_Figure)
                    {
                        return layoutHolder.Room;
                    }
                }

            return emptyLayout.Room;
        }

        private Room_Figure GetRandomLayout(Room_Figure owner, Vector2Int curentExit, bool canBeLarge)
        {
            List<LayoutHolder> holders = new List<LayoutHolder>(LayoutHolders.Length);

            foreach(LayoutHolder holder in LayoutHolders)
            {
                if ((!holder.Room_Figure.isLarge || canBeLarge) && holder.Room_Figure.CanConnect(owner, curentExit))
                {
                    holders.Add(holder);
                }
            }

            sum = 0;
            foreach (LayoutHolder holder in holders)
            {
                sum += holder.weight;
            }

            int randomIndex = Random.Range(0, sum);

            for(int i = 0; i < holders.Count; i++)
            {
                randomIndex -= holders[i].weight;

                if(randomIndex <= 0)
                {
                    return holders[i].Room_Figure;
                }
                
            }

            return holders[0].Room_Figure;
        }

        public Room_Figure GetRandomLayout(RoomType type,Room_Figure owner, Vector2Int curentExit, bool canBeLarge)
        {
            if(type == null)
            {
                return GetRandomLayout(owner, curentExit, canBeLarge);
            }

            if(!type.HasSpecialFigure)
            {
                return emptyLayout.Room_Figure;
            }

            List<LayoutHolder> holders = new List<LayoutHolder>(LayoutHolders.Length);

            if (type.id == 1)
                foreach (LayoutHolder holder in TreasureRoomLayouts)
                {
                    if ((!holder.Room_Figure.isLarge || canBeLarge) && holder.Room_Figure.CanConnect(owner, curentExit))
                    {
                        holders.Add(holder);
                    }
                }

            if (type.id == 0)
                foreach (LayoutHolder holder in BossRoomLayouts)
                {
                    if ((!holder.Room_Figure.isLarge || canBeLarge) && holder.Room_Figure.CanConnect(owner, curentExit))
                    {
                        holders.Add(holder);
                    }
                }

            sum = 0;
            foreach (LayoutHolder holder in holders)
            {
                sum += holder.weight;
            }

            int randomIndex = Random.Range(0, sum);

            for (int i = 0; i < holders.Count; i++)
            {
                randomIndex -= holders[i].weight;

                if (randomIndex <= 0)
                {
                    return holders[i].Room_Figure;
                }

            }

            return holders[0].Room_Figure;
        }

        public Room_Figure GetRandomLayout(RoomType type, Room_Figure owner, Vector2Int ownerPos,
            Vector2Int neighbourPos, bool canBeLarge)
        {
            //if (type == null)
            //{
            //    return GetRandomLayout(type, owner, curentExit, canBeLarge);
            //}

            if (!type.HasSpecialFigure)
            {
                return emptyLayout.Room_Figure;
            }

            List<LayoutHolder> holders = new List<LayoutHolder>(LayoutHolders.Length);

            if (type.id == 1)
                foreach (LayoutHolder holder in TreasureRoomLayouts)
                {
                    if ((!holder.Room_Figure.isLarge || canBeLarge) && holder.Room_Figure.CanConnect(owner, ownerPos, neighbourPos))
                    {
                        holders.Add(holder);
                    }
                }

            if (type.id == 0)
                foreach (LayoutHolder holder in BossRoomLayouts)
                {
                    if ((!holder.Room_Figure.isLarge || canBeLarge) && holder.Room_Figure.CanConnect(owner, ownerPos, neighbourPos))
                    {
                        holders.Add(holder);
                    }
                }

            sum = 0;
            foreach (LayoutHolder holder in holders)
            {
                sum += holder.weight;
            }

            int randomIndex = Random.Range(0, sum + 1);

            for (int i = 0; i < holders.Count; i++)
            {
                randomIndex -= holders[i].weight;

                if (randomIndex <= 0)
                {
                    return holders[i].Room_Figure;
                }

            }

            if(holders.Count == 0)
            {
                Debug.LogError("Picker Error");
                return emptyLayout.Room_Figure;
            }

            return holders[0].Room_Figure;
        }

        [System.Serializable]
        public class LayoutHolder
        {
            public Room_Figure Room_Figure;

            public Room_GM Room;

            public int weight;
        }
    }
}