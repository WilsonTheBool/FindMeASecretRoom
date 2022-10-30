using Assets.Scripts.Game.Gameplay.SecretRoomBook;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Tutorial
{
    [System.Serializable]
    public class TutorialLevelHolder : MonoBehaviour
    {
        public Transform levelHolder;

        public int tutorialChapterID;

        public bool OpenBookOnStart;
       
        public Room_GM_TypeHolder[] GetLevel()
        {
            return levelHolder.GetComponentsInChildren<Room_GM_TypeHolder>();
        }
    }
}