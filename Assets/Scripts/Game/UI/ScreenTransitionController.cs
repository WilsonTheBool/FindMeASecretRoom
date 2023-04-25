using Assets.Scripts.Game.Pregression;
using Assets.Scripts.LevelGeneration;
using System.Collections;
using System.Collections.Generic;
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

        public LevelToTransitionObject interpreter;

        private Color savedColor;

        public void SetUp(GameProgressionController controller)
        {
            for(int i = 0; i < roomsHolder.childCount; i++)
            {
                Destroy(roomsHolder.GetChild(i).gameObject);
            }
            
            roomsHolder.DetachChildren();

            List<TransitionRoom> instance_rooms = new List<TransitionRoom>();

            for (int i = 0; i < controller.compainData.levels.Length -1; i++)
            {
                if (controller.compainData.levels[i].level != null)
                    savedColor = controller.compainData.levels[i].level.baseColor;

                var room = interpreter.GetTransitionRoom(controller.compainData.levels[i].levelAction);


                if (room != null)
                {
                    TransitionRoom instance = Instantiate(room, roomsHolder);
                    instance.SetColor(savedColor);
                    instance_rooms.Add(instance);
                }
            }

            
            instance_rooms.Add(Instantiate(interpreter.bossRoom, roomsHolder));

            //if(instance_rooms[instance_rooms.Count - 1].bridge != null)
            //{
            //    instance_rooms[instance_rooms.Count - 1].bridge.gameObject.SetActive(false);
            //}

        }

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