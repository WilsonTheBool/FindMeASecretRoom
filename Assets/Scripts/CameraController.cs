using System.Collections;
using UnityEngine;
using Assets.Scripts.InputManager;

namespace Assets.Scripts
{
    public class CameraController : MonoBehaviour
    {
        public InputListener input;

        public float speed;

        Camera cam;

        public Vector3 mousePrev;

        CameraBoundsController cameraBoundsController;

        private void Awake()
        {
            cameraBoundsController = GetComponent<CameraBoundsController>();
            cam = GetComponent<Camera>();
            input.OnMove.AddListener(OnMove);
            input.OnMoveStart.AddListener(OnMoveStart);
        }

        private void OnMoveStart()
        {
            mousePrev = input.worldMousePosition;
        }
        private void OnMove()
        {
            Vector3 vec = (input.worldMousePosition - mousePrev);
            vec.z = 0;
            cam.transform.Translate(-speed * Time.deltaTime * vec);
            cam.transform.position = cameraBoundsController.GetCamPosition(cam.transform.position);
            mousePrev = input.worldMousePosition;
        }
    }
}