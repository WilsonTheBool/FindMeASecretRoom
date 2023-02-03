using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Game.Items.UseBehaviours
{
    public class Shovel_Behaviour : ItemUseBehaviour
    {
        public Sprite shovelSprite;

        public override bool CanUse(Item.ItemInternalEventArgs args)
        {
            //return args.external.mainGameController.progression.leveldataCount < args.external.mainGameController.progression.levels.Length - 1;
            return true;
        }

        public override void OnAlternativeUse(Item.ItemInternalEventArgs args)
        {
           
        }

        public override void OnDeselected(Item.ItemInternalEventArgs args)
        {
            args.external.mainGameController.GameSelectTileController.ReturnToDefaultSprite();
        }

        public override void OnSelected(Item.ItemInternalEventArgs args)
        {
            args.external.mainGameController.GameSelectTileController.SetTileSprite(shovelSprite);
        }

        public override void OnTilePosChnaged(Item.ItemInternalEventArgs args)
        {
            
        }

        public override void OnUse(Item.ItemInternalEventArgs args)
        {
            args.external.mainGameController.GameSelectTileController.ReturnToDefaultSprite();

            

            args.external.player.itemsController.RemoveItem(args.item, args.external);

            ItemUsed.Invoke();

            args.external.mainGameController.LevelMapRenderer.ClearAll();

            args.external.mainGameController.GameTilemapController.ClearAll();

            args.external.mainGameController.onVictory.Invoke();
        }
    }
}