using System.Collections;
using UnityEngine;

namespace Assets.Scripts.MainMenu
{
    public class FloatInDirection : MonoBehaviour
    {

        public float speed;

        public Vector3 direction;

        public void SetTimer(float destroyTime)
        {
            Destroy(gameObject, destroyTime);
        }

        private void Update()
        {
            transform.position += speed * Time.deltaTime * direction;
        }
    }
}