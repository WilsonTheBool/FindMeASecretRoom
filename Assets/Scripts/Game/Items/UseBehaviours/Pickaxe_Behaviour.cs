using UnityEditor;
using UnityEngine;
using Assets.Scripts.Game.Items;
using Assets.Scripts.Game.Gameplay;
using Assets.Scripts.Game.PlayerController;
using System.Collections.Generic;
using Assets.Scripts.LevelGeneration;
using Assets.Scripts.Game.GameMap;

namespace Assets.Scripts.Game.Items.UseBehaviours
{
    public class Pickaxe_Behaviour : ItemUseBehaviour
    {
        private PlayerHPController playerHPController;

        [SerializeField]
        private int damageOnItemMiss = 1;

        [SerializeField]
        private GameObject smokePrefab;

        [SerializeField]
        private Sprite PickAxesSlectTileSprite;

        MainGameLevelMapController main;

        private void Start()
        {
            main = MainGameLevelMapController.Instance;
            main.onLevelOver.AddListener(OnLevelOver);
            main.GameMapRoomUnlockController.roomUnlocked.AddListener(OnUnlocked);
            main.GameMapRoomUnlockController.roomUnlocked.AddListener(OnUnlocked);
            flags = new Dictionary<Vector2Int, GameObject>();

        }

        public override bool CanUse(Item.ItemInternalEventArgs args)
        {
            if (args.external.mainGameController.GameMapRoomUnlockController.IsUnlocked(args.external.tilePos))
            {
                return false;
            }

            if (args.external.mainGameController.GameTilemapController.IsMarked(args.external.tilePos))
            {
                return false;
            }

            return args.external.mainGameController.GameMapRoomUnlockController.CanCheckToUnlock(args.external.tilePos);
        }

        public override void OnUse(Item.ItemInternalEventArgs args)
        {
            Explosion explosion = new Explosion()
            {
                range = 0,
                position = args.external.tilePos,
                ExplosionController = args.external.mainGameController.ExplosionController,
                type = Explosion.RangeType.square,
            };

            playerHPController = args.external.player.playerHPController;

            explosion.onAfterExplosion += Explosion_onAfterExplosion;

            explosion.Explode_Fake();

            Destroy(args.external.mainGameController.LevelMapRenderer.CreateObject(smokePrefab, explosion.position), 0.8f);

            args.external.mainGameController.GameSelectTileController.ReturnToDefaultSprite();

            ItemUsed.Invoke();
        }

        private void Explosion_onAfterExplosion(Explosion arg1, ExplosionResult arg2)
        {
            if (arg2 != null && arg2.secretRoomsUnlocked <= 0)
            {
                playerHPController.RequestTakeDamage(new PlayerHPController.HpEventArgs(damageOnItemMiss, this.gameObject));
            }

            foreach (Vector2Int pos in arg2.areaHit)
            {
                main.GameTilemapController.RemoveFlag(pos);
            }

        }


        private Dictionary<Vector2Int,GameObject> flags;

        [SerializeField]
        private GameObject flag_prefab;

        public override void OnAlternativeUse(Item.ItemInternalEventArgs args)
        {
            if (main.GameTilemapController.CanPlaceFlag(args.external.tilePos))
            {
                main.GameTilemapController.AddFlag(args.external.tilePos);
            }
            else
            {
                main.GameTilemapController.RemoveFlag(args.external.tilePos);
            }

            ItemUsed_Alt.Invoke();
        }

        private void OnLevelOver()
        {

        }

        private void OnUnlocked(Room room)
        {


        }

        public override void OnDeselected(Item.ItemInternalEventArgs args)
        {
            args.external.mainGameController.GameSelectTileController.ReturnToDefaultSprite();
        }

        public override void OnSelected(Item.ItemInternalEventArgs args)
        {
            OnTilePosChnaged(args);
        }

        public override void OnTilePosChnaged(Item.ItemInternalEventArgs args)
        {
            if (CanUse(args))
            {
                args.external.mainGameController.GameSelectTileController.SetTileSprite(PickAxesSlectTileSprite);
            }
            else
            {
                args.external.mainGameController.GameSelectTileController.SetEmptySprite();
            }
        }
    }
}