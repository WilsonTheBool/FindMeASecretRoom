using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Tutorial
{
    public class FlipTargetActive : MonoBehaviour
    {
        public GameObject target;
        public void Flip()
        {
            target.SetActive(!target.activeSelf);
        }
    }
}