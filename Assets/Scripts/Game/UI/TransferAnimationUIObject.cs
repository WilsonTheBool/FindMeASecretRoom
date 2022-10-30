using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Game.UI
{
    public class TransferAnimationUIObject : MonoBehaviour
    {
        public SpriteRenderer SpriteRenderer;

        public float speed;
        public float scaleChangeSpeed;

        private Vector3 destination;
        private Vector3 endScale;

        private bool isTransfering;

        public UnityEvent onTargetReached;

        public void StartTransfer(Vector3 destination, Vector3 endScale)
        {
            this.destination = destination;
            this.endScale = endScale;
            isTransfering = true;
        }

        private void Update()
        {
            if (isTransfering)
            {
                Vector3 newPos = Vector3.Lerp(transform.position, destination, speed * Time.deltaTime);
                transform.position = newPos;

                Vector3 newScale = Vector3.Lerp(transform.localScale, endScale, scaleChangeSpeed * Time.deltaTime);
                transform.localScale = newScale;

                if(Vector3.Distance(newPos, destination) <= 0.1f)
                {
                    onTargetReached.Invoke();
                    Destroy(this.gameObject);
                }
            }
        }
    }
}