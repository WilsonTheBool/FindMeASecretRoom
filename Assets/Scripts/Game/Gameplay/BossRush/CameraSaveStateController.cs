using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Game.Gameplay.BossRush
{
    [RequireComponent(typeof(Camera))]
    public class CameraSaveStateController : MonoBehaviour
    {

        Camera Camera;

        private void Awake()
        {
            Camera = GetComponent<Camera>();
        }

        public void Save(ref SaveData saveData)
        {

            saveData.cameraPos = this.Camera.transform.position;
            saveData.cameraSize = Camera.orthographicSize;
        }

        public bool Load(ref SaveData data)
        {
            if (data == null)
            {
                return false;
            }

            Camera.transform.position = data.cameraPos;
            Camera.orthographicSize = data.cameraSize;

            return true;
        }

        public class SaveData
        {
            public  Vector3 cameraPos;

            public float cameraSize;
        }
    }
}