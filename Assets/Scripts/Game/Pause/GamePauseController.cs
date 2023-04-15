using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Game.Pause
{
    public class GamePauseController : MonoBehaviour
    {
        public static GamePauseController Instance;

        public static UnityEvent<bool> onPauseToggle;

        private static bool isPaused;

        private static float timescale;

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
                timescale = Time.timeScale;

                if(onPauseToggle == null)
                onPauseToggle = new UnityEvent<bool>();
            }
        }

        private void OnDestroy()
        {
            if(Instance == this)
            {
                if (isPaused)
                {
                    Resume();
                }
            }
        }

        public static void Pause()
        {
            

            if (!isPaused)
            {
                isPaused = true;
                timescale = Time.timeScale;

                Time.timeScale = 0f;

                onPauseToggle?.Invoke(isPaused);
            }
            
        } 

        public static void Resume()
        {
            if (isPaused)
            {
                isPaused = false;

                Time.timeScale = timescale;

                onPauseToggle?.Invoke(isPaused);
            }
            
        }
    }
}