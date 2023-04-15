using Assets.Scripts.Game.GameMap;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Game.UI.CameraMovement
{
    public class MoveCameraField : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        UICameraMovementController m_Controller;

        public Vector3 direction;

        public float speed;

        bool isActivated;

        private void Update()
        {
            if(isActivated)
            m_Controller.MoveCamera(direction * speed);
        }

        public void SetUp(UICameraMovementController controller)
        {
            m_Controller = controller;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            isActivated = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isActivated = false;
        }
    }
}