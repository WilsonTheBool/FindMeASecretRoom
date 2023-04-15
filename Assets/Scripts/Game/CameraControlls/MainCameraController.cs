using Assets.Scripts.Game.GameMap;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Game.CameraControlls
{
    public class MainCameraController : MonoBehaviour
    {
        public bool isMoving;

        Camera cam;
        CameraController CameraController;

        public float maxDeltaIgnore;
        public float DeltaIgnore;


        private void Start()
        {
            cam = MainGameLevelMapController.Instance.cam;
            CameraController = cam.GetComponentInChildren<CameraController>();
        }

        public void On_Alt(InputAction.CallbackContext callbackContext)
        {
            if (callbackContext.canceled)
            {
                DeltaIgnore = 0;
            }
        }

        public void On_MoveStart(InputAction.CallbackContext callbackContext)
        {
            if (callbackContext.started)
            {
                isMoving = true;
                DeltaIgnore = maxDeltaIgnore;
            }
            else
            {
                isMoving = false;
                DeltaIgnore = 0;
            }

            
        }

       public void On_MoveEvent(InputAction.CallbackContext callbackContext)
        {
            if (isMoving)
            {
                
                Vector2 value = callbackContext.ReadValue<Vector2>();

                if(value.magnitude > DeltaIgnore)
                CameraController.Move(value);
            }

        }
    }
}