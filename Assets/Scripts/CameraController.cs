using System.Collections;
using UnityEngine;
using Assets.Scripts.InputManager;
using UnityEngine.InputSystem;

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

        private void LateUpdate()
        {
        //    if (Input.GetMouseButtonDown(1))
        //    {
        //        OnMoveStart();
        //    }

        //    if (Input.GetMouseButton(1))
        //    {
        //        OnMove();
        //    }

            OnMove();
        }

        private void OnMoveStart()
        {
            mousePrev = input.viewportMousePos;
            print(mousePrev);
        }
        private void OnMove()
        {
            //Vector3 vec = (input.viewportMousePos - mousePrev);

            //vec.z = 0;
            //Vector3 tr = -speed * Time.deltaTime * vec;
            //cam.transform.Translate(tr);
            //cam.transform.position = cameraBoundsController.GetCamPosition(cam.transform.position);
            //mousePrev = input.viewportMousePos;

            float vertical = Input.GetAxisRaw("Vertical");
            float horizontal = Input.GetAxisRaw("Horizontal");

            Vector3 tr = new Vector3(horizontal, vertical, 0);
            tr.Normalize();
            tr *= speed * Time.deltaTime;

            cam.transform.position = cameraBoundsController.GetCamPosition(cam.transform.position + tr);
        }
    }
}