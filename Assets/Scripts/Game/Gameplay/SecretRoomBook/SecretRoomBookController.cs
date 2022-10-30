using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Game.Gameplay.SecretRoomBook
{
    public class SecretRoomBookController : MonoBehaviour
    {
        public static SecretRoomBookController Instance;

        public SecretRoomBookUI Book;

        public ChapterHolder[] chapters;

        protected int curentChapterID;
        protected int curentPageID;

        public bool isMainController = true;

        private void Awake()
        {
            if(isMainController)
                if(Instance == null)
                {
                    Instance = this;
                }
                else
                {
                    Destroy(Instance);
                    Instance = this;
                }

            if(Book == null)
            Book = FindObjectOfType<SecretRoomBookUI>();

            Book.OnNextPage.AddListener(OnNextPage);
            Book.OnPreviousPage.AddListener(OnPreviousPage);
            Book.OnChapterSelect.AddListener(OpenBook);
            Book.OnBookClose.AddListener(CloseBook);
        }

        public virtual bool HasNextPage()
        {
            if(TryGetChapter(curentChapterID, out ChapterHolder chapterHolder))
            {
                if(curentPageID >= 0 && curentPageID < chapterHolder.pages.Length - 1)
                {
                    return true;
                }
            }

            return false;
        }

        public virtual bool HasPrevPage()
        {
            if (TryGetChapter(curentChapterID, out ChapterHolder chapterHolder))
            {
                if (curentPageID > 0 && curentPageID < chapterHolder.pages.Length)
                {
                    return true;
                }
            }

            return false;
        }

        private void Start()
        {
            CloseBook();
        }

        public void OnNextPage()
        {
            TryOpenPage(curentPageID + 1);
        }
        public void OnPreviousPage()
        {
            TryOpenPage(curentPageID - 1);
        }

        public bool TryOpenPage(int id)
        {
            print("Page " + id);
            if (TryGetChapter(curentChapterID, out ChapterHolder holder))
            {
                if (id >= 0 && id < holder.pages.Length)
                {
                    curentPageID = id;
                    Book.LoadPage(holder.pages[id], HasNextPage(), HasPrevPage());
                    return true;
                }
            }

            return false;
        }

        public void CloseBook()
        {
            Book.gameObject.SetActive(false);
        }

        public void OpenBook()
        {
            Book.gameObject.SetActive(true);
            curentChapterID = 0;
            OpenBook(curentPageID);
        }

        public void OpenBook(int chapterId)
        {
            if (TryGetChapter(chapterId, out ChapterHolder holder))
            {
                Book.gameObject.SetActive(true);
                curentChapterID = chapterId;
                curentPageID = 0;
                Book.LoadPage(holder.pages[0], HasNextPage(), HasPrevPage());
            }
            else
            {
                return;
            }
        }

        public void OpenBook(ChapterType type)
        {
            if(TryGetChapter(type, out ChapterHolder holder))
            {
                Book.gameObject.SetActive(true);
                curentChapterID = holder.id;
                curentPageID = 0;
                Book.LoadPage(holder.pages[0], HasNextPage(), HasPrevPage());
            }
            else
            {
                return;
            }
            
        }

        public void Open_Close_Book(int chapter)
        {
            if(Book != null)
            {
                if (Book.gameObject.activeSelf)
                {
                    CloseBook();
                }
                else
                {
                    OpenBook(chapter);
                }
            }
        }

        private bool TryGetChapter(int id, out ChapterHolder chapter)
        {
            if(id >= 0 && id < chapters.Length)
            {
                chapter = chapters[id];
                return true;
            }

            chapter = new ChapterHolder();
            return false;
            
        }

        private bool TryGetChapter(ChapterType type, out ChapterHolder chapter)
        {
            foreach(ChapterHolder chapterHolder in chapters)
            {
                if(chapterHolder.type == type)
                {
                    chapter = chapterHolder;
                    return true;
                }
            }

            chapter = new ChapterHolder();
            return false;

        }

        public void Request_OpenOprions()
        {
            OpenBook(ChapterType.options);
        }

        [System.Serializable]
        public struct ChapterHolder
        {
            public int id;

            public ChapterType type;

            public SecretBookPage[] pages;
        }

        public enum ChapterType 
        {
            rooms, options, menu, tutorial
        }
    }
}