using Assets.Scripts.Game.GameMap;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game.UI.CameraMovement
{
    public class UICameraMovementController: MonoBehaviour
    {
        private MoveCameraField[] fields;

        public float speed;

        private CameraController Camera;

        private void Awake()
        {
            fields = GetComponentsInChildren<MoveCameraField>();
        }

        private void Start()
        {
            Camera = MainGameLevelMapController.Instance.cam.GetComponentInChildren<CameraController>();
        
            foreach(var field in fields)
            {
                field.SetUp(this);
            }

        }

        public void MoveCamera(Vector3 direction)
        {
            Camera.Move(direction * speed);
        }
    }
}
