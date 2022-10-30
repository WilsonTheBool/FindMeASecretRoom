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
        public Sprite EmptySprite;

        public GameLevelInputManager InputListener;

        public GameMapRoomUnlockController GameMapRoomUnlockController;

        private Sprite curentSprite;

        private void Awake()
        {
            spriteRenderer = selectTile.GetComponentInChildren<SpriteRenderer>();

            InputListener.TilePositionChanged.AddListener(UpdateSelectTile);
        }

        //private void Start()
        //{
        //    UpdateSelectTile();
        //}

       
        public void SetTileSprite(Sprite sprite)
        {
            if(spriteRenderer.sprite != cantCheckSprite)
            {
                curentSprite = sprite;
                spriteRenderer.sprite = curentSprite;
            }
            else
            {
                curentSprite = sprite;
            }

           

            
        }

        public void ReturnToDefaultSprite()
        {
           

            if (spriteRenderer.sprite != cantCheckSprite)
            {
                curentSprite = normalSprite;
                spriteRenderer.sprite = curentSprite;
            }
            else
            {
                curentSprite = normalSprite;
            }
        }

        public void SetEmptySprite()
        {
            

            if (spriteRenderer.sprite != cantCheckSprite)
            {
                curentSprite = EmptySprite;
                spriteRenderer.sprite = curentSprite;
            }
            else
            {
                curentSprite = EmptySprite;
            }
        }

        private void UpdateSelectTile()
        {
            selectTile.transform.position = InputListener.curentCellCeneter;

            if(GameMapRoomUnlockController == null)
            {
                return;
            }

            if (GameMapRoomUnlockController.CanCheckToUnlock(InputListener.CurentTileMousePosition))
            {
                spriteRenderer.sprite = curentSprite;
            }
            else
            {
                spriteRenderer.sprite = cantCheckSprite;
            }
        }
    }
}
