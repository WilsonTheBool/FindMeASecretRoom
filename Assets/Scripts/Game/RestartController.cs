using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Game
{
    public class RestartController : MonoBehaviour
    {
        public SceneLoader mainLoad;

        public float holdTimer;

        private float curentHold = 0;
        private void Update()
        {

            if (Input.GetKey(KeyCode.R))
            {
                curentHold += Time.deltaTime;
            }

            if (Input.GetKeyUp(KeyCode.R))
            {
                curentHold = 0;
            }

            if(curentHold > holdTimer)
            {
                Debug.Log("RESTART");
                mainLoad.LoadScene();
            }
        }
    }
}