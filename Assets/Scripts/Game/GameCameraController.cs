using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game
{
    public class GameCameraController: MonoBehaviour
    {
        public Camera Camera;

        public float maxCameraZoomValue;
        public float minCameraZoomValue;

        public float cameraZoomSpeed;

        private void Awake()
        {
            Camera = Camera.main;
        }

        private void Update()
        {
            Vector2 scroll = Input.mouseScrollDelta;
            if (scroll.y != 0)
            {
                Camera.orthographicSize -= cameraZoomSpeed * scroll.y * Time.fixedDeltaTime;

                Camera.orthographicSize = Mathf.Clamp(Camera.orthographicSize, minCameraZoomValue, maxCameraZoomValue);
            }

            
        }
    }
}
