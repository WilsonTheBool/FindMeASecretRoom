using Assets.Scripts.Game.Pregression;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Game.UI
{
    public class ScreenTransitionController : MonoBehaviour
    {
        public float fadeSpeed;

        public float transitionDuration;

        public CanvasGroup CanvasGroup;

        public Transform roomsHolder;

        public UnityEvent FadeInEnded;
        public UnityEvent FadeOutEnded;

        public TMPro.TMP_Text levelNameText;

        public Transform youAreHere;

        public Vector3 pointerOffset;

        public bool isFadeInCompleted = false;
        private bool isFadeOutStarted = false;


        public void StartFadeIn(int curentRoom, string levelName)
        {
            gameObject.SetActive(true);

            levelNameText.text = levelName;


            if(curentRoom >= 0 && curentRoom < roomsHolder.childCount)
            {
                youAreHere.transform.SetParent(roomsHolder.GetChild(curentRoom));
                youAreHere.transform.localPosition = pointerOffset;
            }


            StartCoroutine(FadeAnim());
        }

        public void CancelFadeOut()
        {
            if (!isFadeOutStarted)
            {
                StopAllCoroutines();
                StartCoroutine(FadeOut());
            }

        }

        private IEnumerator FadeOut()
        {
            isFadeOutStarted = true;

            while (CanvasGroup.alpha > 0)
            {
                CanvasGroup.alpha -= fadeSpeed * Time.deltaTime;
                yield return null;
            }

            EndFade();

            FadeOutEnded.Invoke();
        }

        private void EndFade()
        {
            CanvasGroup.alpha = 0;
            gameObject.SetActive(false);
            isFadeInCompleted = false;
            isFadeOutStarted = false;
        }



        private IEnumerator FadeAnim()
        {
            while(CanvasGroup.alpha < 1)
            {
                CanvasGroup.alpha += fadeSpeed * Time.deltaTime;
                yield return null;
            }

            yield return new WaitForEndOfFrame();

            isFadeInCompleted = true;
            FadeInEnded.Invoke();

            yield return new WaitForSeconds(transitionDuration);

            isFadeOutStarted = true;

            while (CanvasGroup.alpha > 0)
            {
                CanvasGroup.alpha -= fadeSpeed * Time.deltaTime;
                yield return null;
            }

            EndFade();

            FadeOutEnded.Invoke();
            
        }



    }
}