using Assets.Scripts.Game.GameMap;
using Assets.Scripts.Game.Gameplay.SecretRoomBook;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Tutorial
{
    public class MainTutorialController : MonoBehaviour
    {
        public MainGameLevelMapController main;

        public TutorialLevelHolder[] tutorialLevelHolders;

        private int curentLevelID = 0;

        public SecretRoomBookController tutorialBook;


        private void Awake()
        {
            if(main == null)
            {
                main = FindObjectOfType<MainGameLevelMapController>();
            }

            main.onVictory.AddListener(() => curentLevelID++);
            main.LevelMapRenderer.onRenderEnded.AddListener(SetUp);
            main.onDefeat.AddListener(Restart);
        }

        public Room_GM_TypeHolder[] GetCurentLevel()
        {
            if (curentLevelID >= 0 && curentLevelID < tutorialLevelHolders.Length)
            {
               return tutorialLevelHolders[curentLevelID].levelHolder.GetComponentsInChildren<Room_GM_TypeHolder>(true);
            }

            return null;
        }

        public int GetCurentBookChapter()
        {

            if (curentLevelID >= 0 && curentLevelID < tutorialLevelHolders.Length)
            {
                return tutorialLevelHolders[curentLevelID].tutorialChapterID;
            }

            return -1;
        }

        public void SetUp()
        {
            main.Player.playerHPController.HealToFullRedHP();

            int chapterId = GetCurentBookChapter();

            if (curentLevelID >= 0 && curentLevelID < tutorialLevelHolders.Length)
            {
                if (!tutorialLevelHolders[curentLevelID].OpenBookOnStart)
                {
                    return;
                }
            }
            if (chapterId != -1)
            {
                tutorialBook.OpenBook(chapterId);
            }


            
        }

        public void Restart()
        {
            main.RestartLevel();
        }

        public void OpenCloseTutorialBook()
        {
            if (tutorialBook.Book != null)
            {
                if (tutorialBook.Book.gameObject.activeSelf)
                {
                    tutorialBook.CloseBook();
                }
                else
                {
                    int chapterId = GetCurentBookChapter();
                    if (chapterId != -1)
                    {
                        tutorialBook.OpenBook(chapterId);
                    }
                }
            }
        }
    }
}