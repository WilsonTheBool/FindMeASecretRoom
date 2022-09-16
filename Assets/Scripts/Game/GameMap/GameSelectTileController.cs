using Assets.Scripts.InputManager;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game.GameMap
{
    public class GameSelectTileController: MonoBehaviour
    {
        public GameObject selectTile;

        private SpriteRenderer spriteRenderer;

        public Sprite normalSprite;
        public Sprite cantCheckSprite;

        public GameLevelInputManager InputListener;

        public GameMapRoomUnlockController GameMapRoomUnlockController;


        private void Awake()
        {
            spriteRenderer = selectTile.GetComponentInChildren<SpriteRenderer>();

            InputListener.TilePositionChanged.AddListener(UpdateSelectTile);
        }

        private void Start()
        {
            UpdateSelectTile();
        }

        private void UpdateSelectTile()
        {
            selectTile.transform.position = InputListener.curentCellCeneter;

            if (GameMapRoomUnlockController.CanCheckToUnlock(InputListener.CurentTileMousePosition))
            {
                spriteRenderer.sprite = normalSprite;
            }
            else
            {
                spriteRenderer.sprite = cantCheckSprite;
            }
        }
    }
}
