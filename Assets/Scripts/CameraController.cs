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

        //public float maxVelosity;

        Camera cam;

        CameraBoundsController cameraBoundsController;

        private void Awake()
        {
            cameraBoundsController = GetComponent<CameraBoundsController>();
            cam = GetComponent<Camera>();
        }

        private void LateUpdate()
        {
            //OnMove();
        }

        //private void OnMove()
        //{
        //    float vertical = Input.GetAxisRaw("Vertical");
        //    float horizontal = Input.GetAxisRaw("Horizontal");

        //    Vector3 tr = new Vector3(horizontal, vertical, 0);
        //    tr.Normalize();
        //    tr *= speed * Time.deltaTime;

        //    cam.transform.position = cameraBoundsController.GetCamPosition(cam.transform.position + tr);
        //}

        public bool useCamSize;

        public void Move(Vector3 tr)
        {
            if(useCamSize)
            tr *= speed * (cam.orthographicSize/10) * Time.fixedDeltaTime;
            else
            tr *= speed * Time.fixedDeltaTime;
            //if(tr.magnitude > maxVelosity)
            //{
            //    tr = tr.normalized * maxVelosity;
            //}

            //tr *= Time.deltaTime;

            cam.transform.position = cameraBoundsController.GetCamPosition(cam.transform.position + tr);
        }
    }
}