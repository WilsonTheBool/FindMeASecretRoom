using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.Game.Gameplay.SecretRoomBook
{
    public class SecretRoomBookUI : MonoBehaviour
    {
        public UnityEvent OnNextPage;
        public UnityEvent OnPreviousPage;

        public UnityEvent OnBookClose;

        public ChapterEvent OnChapterSelect;

        public SecretBookPage curentPage;

        public Button nextButton;
        public Button prevButton;

        public Transform pageHolder;

        public void LoadPage(SecretBookPage page, bool hasnext, bool hasPrev)
        {
            if (curentPage != null)
            {
                Destroy(curentPage.gameObject);
            }

            if(page != null)
            {
                curentPage = Instantiate(page, pageHolder);

                nextButton.gameObject.SetActive(hasnext);
                prevButton.gameObject.SetActive(hasPrev);
                
            }
           
        }

        public void Request_CloseBook()
        {
            OnBookClose.Invoke();
        }

        public void Activate_NextPage()
        {
            OnNextPage.Invoke();
        }

        public void Activate_PrevPage()
        {
            OnPreviousPage.Invoke();
        }

        public void Activate_SelecetChapter(int id)
        {
            OnChapterSelect.Invoke(id);
        }

        [System.Serializable]
        public class ChapterEvent: UnityEvent<int>
        {

        }
    }
}