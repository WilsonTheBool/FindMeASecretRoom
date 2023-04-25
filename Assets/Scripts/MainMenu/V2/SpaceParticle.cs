using System.Collections;
using UnityEngine;

namespace Assets.Scripts.MainMenu.V2
{
    public class SpaceParticle : MonoBehaviour
    {
        public Transform despawnPoint;
        public Transform spawnPoint;

        public Vector3 PositionVelocity;

        public Vector3 RotationVelocity;

        private void Update()
        {
            transform.position += PositionVelocity * Time.deltaTime;
            transform.Rotate(RotationVelocity * Time.deltaTime);
        }

        private void FixedUpdate()
        {
            if (transform.position.y >= despawnPoint.position.y)
            {
                transform.position = new Vector3(transform.position.x, spawnPoint.position.y, transform.position.z);
            }
        }
    }
}