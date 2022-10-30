using Assets.Scripts.LevelGeneration;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Game.Gameplay.SecretRoomBook
{
    [CreateAssetMenu(menuName = "Book/Room To Page Taranslator")]
    public class RoomToBookPageTanslator : ScriptableObject
    {
        public RoomToPageHolder[] holders;

        public int GetRoomPage(RoomType roomType)
        {
            foreach(RoomToPageHolder roomToPageHolder in holders)
            {
                if(roomToPageHolder.RoomType == roomType)
                {
                    return roomToPageHolder.pageId;
                }
            }

            return -1;
        }

        [System.Serializable]
        public struct RoomToPageHolder
        {
            public RoomType RoomType;

            public int pageId;
        }
    }
}