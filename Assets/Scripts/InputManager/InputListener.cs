using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Assets.Scripts.InputManager
{
    public class InputListener : MonoBehaviour
    {
        public string ListenerName;

        public InputManager_SO InputManager_SO;

        public bool passInputDown;

        public int InputGroup;

        public UnityEvent OnActivate;

        public UnityEvent OnCancel;

        public UnityEvent OnAccept;

        public UnityEvent OnAlternativeAction;

        public UnityEvent OnMoveStart;
        public UnityEvent OnMove;

        public UnityEvent OnEscape;

        public UnityEvent OnOpenBook;

        public Vector3 mousePosition;
        public Vector3 worldMousePosition;
        public Vector3 viewportMousePos;


        private void Awake()
        {
            InputManager_SO.AddInputListener(this);
        }

        private void OnDestroy()
        {
            InputManager_SO.RemoveInputListener(this);
        }

        private void OnEnable()
        {
            InputManager_SO.AddInputListener(this);
        }

        private void OnDisable()
        {
            InputManager_SO.RemoveInputListener(this);
        }

        public void OnInputUpdate(InputUpdateEventArgs args)
        {
            this.mousePosition = args.mousePosition;
            this.worldMousePosition = args.worldMousePosition;
            this.viewportMousePos = Camera.main.ScreenToViewportPoint(mousePosition);
        }

        public class InputUpdateEventArgs: EventArgs
        {
            public Vector3 mousePosition;
            public Vector3 worldMousePosition;

        }
    }
}