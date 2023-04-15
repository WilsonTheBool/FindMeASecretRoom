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

        private Predicate<Vector2Int> CurentCheckFunc;

        private void Awake()
        {
            spriteRenderer = selectTile.GetComponentInChildren<SpriteRenderer>();

            InputListener.TilePositionChanged.AddListener(UpdateSelectTile);

            CurentCheckFunc = Default_Check;
        }

        //private void Start()
        //{
        //    UpdateSelectTile();
        //}

        private bool Default_Check(Vector2Int pos)
        {
            return GameMapRoomUnlockController.CanCheckToUnlock(pos);
        }
       
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

        public void SetPredicate_CanChectTile(Predicate<Vector2Int> predicate)
        {
            CurentCheckFunc = predicate;
        }

        public void SetPredicate_Default()
        {
            CurentCheckFunc = Default_Check;
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

            UpdateSelectTile();
        }

        private void UpdateSelectTile()
        {
            if(selectTile == null)
            {
                return;
            }

            selectTile.transform.position = InputListener.curentCellCeneter;

            if(GameMapRoomUnlockController == null)
            {
                return;
            }

            if (CurentCheckFunc(InputListener.CurentTileMousePosition))
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
