using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Assets.Scripts.InputManager
{
    public class InputListener : MonoBehaviour
    {
        public InputManager_SO InputManager_SO;

        public bool passInputDown;

        public int InputGroup;

        public UnityEvent OnActivate;

        public UnityEvent OnCancel;

        public UnityEvent OnAccept;

        public Vector3 mousePosition;
        public Vector3 worldMousePosition;

       
        private void Awake()
        {
            InputManager_SO.AddInputListener(this);
        }

        private void OnDestroy()
        {
            InputManager_SO.RemoveInputListener(this);
        }

        public void OnInputUpdate(InputUpdateEventArgs args)
        {
            this.mousePosition = args.mousePosition;
            this.worldMousePosition = args.worldMousePosition;
        }

        public class InputUpdateEventArgs: EventArgs
        {
            public Vector3 mousePosition;
            public Vector3 worldMousePosition;

        }
    }
}