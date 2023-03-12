using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Assets.Scripts.InputManager
{
    public class InputReader : MonoBehaviour
    {

        public InputManager_SO InputManager_SO;

        public InputListener[] curentListeners;

        public Camera cam;

        EventSystem EventSystem;

        private void Awake()
        {
            if(InputManager_SO != null && InputManager_SO.InputReader == null)
            {
                InputManager_SO.InputReader = this;
            }
            else
            {
                Destroy(this);
            }

            if(cam == null)
            {
                cam = Camera.main;

                if(cam == null)
                {
                    cam = FindObjectOfType<Camera>();
                }
            }

            if(EventSystem == null)
            {
                EventSystem = EventSystem.current;
            }
        }

        private void OnDisable()
        {
            if(InputManager_SO!=null && InputManager_SO.InputReader == this)
            {
                InputManager_SO.InputReader = null;
            }

            InputManager_SO.InputGroupChanged.RemoveListener(OnInputGroupChanged);
        }

       

        private void Start()
        {
            curentListeners = InputManager_SO.GetCurentGroup();
            InputManager_SO.InputGroupChanged.AddListener(OnInputGroupChanged);
        }

        Vector3 worldMousePos;
        Vector3 mousePos;
        private void Update()
        {
            if (EventSystem.IsPointerOverGameObject())
            {
                return;
            }

            mousePos = Input.mousePosition;

            if(cam != null)
            {
                worldMousePos = cam.ScreenToWorldPoint(mousePos);
                worldMousePos.z = 0;
            }
            else
            {
                worldMousePos = Vector3.zero;
            }

            if(curentListeners != null)
            {

                InputListener.InputUpdateEventArgs args = new InputListener.InputUpdateEventArgs
                {
                    mousePosition = mousePos,
                    worldMousePosition = worldMousePos,
                };

                foreach(InputListener listener in curentListeners)
                {
                    listener.OnInputUpdate(args);
                }
            }

            //if (Input.GetMouseButtonDown(1))
            //{
            //    print("MouseDown");
            //    foreach (var listener in curentListeners)
            //    {
            //        listener.OnMoveStart.Invoke();
            //    }
            //}

            //if (Input.GetMouseButton(1))
            //{
            //    OnMove_InputAction(new InputAction.CallbackContext());
            //}
            
        }

        private void OnInputGroupChanged()
        {
            curentListeners = InputManager_SO.GetCurentGroup();
        }

        public void OnActivate_InputAction(InputAction.CallbackContext callbackContext)
        {

            if (!callbackContext.performed)
            {
                return;
            }

            foreach (var listener in curentListeners)
            {
                listener.OnActivate.Invoke();
            }
        }

        public void OnMove_InputAction(InputAction.CallbackContext callbackContext)
        {

            foreach (var listener in curentListeners)
            {
                listener.OnMove.Invoke();
            }


        }

        public void OnAleternativeAction(InputAction.CallbackContext callbackContext)
        {
            if (!callbackContext.performed)
            {
                return;
            }

            foreach (var listener in curentListeners)
            {
                listener.OnAlternativeAction.Invoke();
            }
        }

        public void OnCancel_InputAction(InputAction.CallbackContext callbackContext)
        {

            if (!callbackContext.performed)
            {
                return;
            }


            foreach (var listener in curentListeners)
            {
                listener.OnCancel.Invoke();
            }
        }

        public void OnAccept_InputAction(InputAction.CallbackContext callbackContext)
        {

            if (!callbackContext.performed)
            {
                return;
            }

            foreach (var listener in curentListeners)
            {
                listener.OnAccept.Invoke();
            }
        }
        
        public void OnEscape_InputAction(InputAction.CallbackContext callbackContext)
        {

            if (!callbackContext.performed)
            {
                return;
            }

            foreach (var listener in curentListeners)
            {
                listener.OnEscape.Invoke();
            }
        }

        public void OnOpenBook_InputAction(InputAction.CallbackContext callbackContext)
        {

            if (!callbackContext.performed)
            {
                return;
            }

            foreach (var listener in curentListeners)
            {
                listener.OnOpenBook.Invoke();
            }
        }

        public void OnTrinketUseAction(InputAction.CallbackContext callbackContext)
        {
            if (!callbackContext.performed)
            {
                return;
            }

            foreach (var listener in curentListeners)
            {
                listener.OnTrinketUse.Invoke();
            }
        }
    }
}