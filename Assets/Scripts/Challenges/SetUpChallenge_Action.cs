using Assets.Scripts.Game.GameMap;
using Assets.Scripts.Game.Items;
using Assets.Scripts.Game.PlayerController;
using Assets.Scripts.Game.Pregression;
using Assets.Scripts.Game.Pregression.Actions;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Challenges
{
    [CreateAssetMenu(menuName = "Progression/SetUp_Challenge")]
    public class SetUpChallenge_Action : ProgressionAction
    {
        public ChallengeRunController ChallengeRunController;

        public SkipTransition_Action SkipTransition;

        [SerializeField]
        Progression_LoadTreasureRoom loadTreasureRoom;

        [SerializeField]
        Progression_LoadShop loadShop;

        public override void DoAction(GameProgressionController progression, MainGameLevelMapController main)
        {
            if (ChallengeRunController != null)
                if (ChallengeRunController.CurentChallenge != null)
                {
                    var data = ChallengeRunController.CurentChallenge;

                    //if(data.Compain != null)
                    //{
                    //    progression.compainData = data.Compain;
                    //}
                    //else
                    //{
                    //    progression.compainData = ChallengeRunController.default_compain;
                    //}

                    if (data.HideShops)
                    {
                        HideShops(progression, main);
                    }

                    if (data.HideTreasureRooms)
                    {
                        HideTreasureRooms(progression, main);
                    }

                    Player player = Player.instance;
                    if (data.OverrrideStartHP)
                    {
                        player.playerHPController.maxHpSlotsCount = data.maxHpContainers;
                        player.playerHPController.SetStartHP(data.redHpOnStart);
                    }

                    if (data.OverrideGold)
                    {
                        if (data.GoldOnStart <= player.goldController.maxGold)
                        {
                            player.goldController.gold = data.GoldOnStart;
                            player.goldController.GoldChanged.Invoke(player.goldController);
                        }

                    }

                    if (data.removeItemsFromPlayer)
                    {
                        foreach (Item item in data.itemsToRemove)
                        {
                            player.itemsController.RemoveItem(item.Name, new Item.ItemExternalEventArgs() { player = player, mainGameController = main });
                        }
                    }

                    if (data.AddItemsOnStart)
                    {
                        foreach (Item item in data.itemsToAdd)
                        {
                            Item instance = Instantiate(item, player.transform);

                            player.itemsController.AddItem(instance, new Item.ItemExternalEventArgs() { player = player, mainGameController = main });

                            progression.ItemPoolController.OnItemPooled(item);
                        }
                    }

                    if (data.RemoveItemsFromPool)
                    {
                        foreach (Item item in data.itemsToPool)
                        {
                            progression.ItemPoolController.OnItemPooled(item);
                        }
                    }



                    if (data.AddTrinket)
                    {
                        player.trinketController.SetTrinket(data.Trinket);
                    }

                    if (data.OverrideRoomReward)
                    {
                        player.roomRewardController.baseRoomReward = data.baseReward;
                        player.roomRewardController.rewardIncreasePerCombo = data.rewardPerCombo;
                    }
                }
                else
                {
                    //progression.compainData = ChallengeRunController.default_compain;
                }

            ChallengeRunController.StartChallenge(progression);

            
        }

        private void HideTreasureRooms(GameProgressionController progression, MainGameLevelMapController main)
        {
            var rooms = progression.TransitionScreenInput.gameObject.GetComponentsInChildren<TransitionRoom>();

            foreach(TransitionRoom room in rooms)
            {
                if(room.type == TransitionRoom.Type.treasure)
                {
                    Destroy(room.gameObject);
                }
            }



            for(int i = 0; i < progression.compainData.levels.Length; i++)
            {
                if (progression.compainData.levels[i].levelAction == loadTreasureRoom)
                {
                    progression.compainData.levels[i].levelAction = SkipTransition;
                }
            }
        }

        private void HideShops(GameProgressionController progression, MainGameLevelMapController main)
        {
            var rooms = progression.TransitionScreenInput.gameObject.GetComponentsInChildren<TransitionRoom>();

            foreach (TransitionRoom room in rooms)
            {
                if (room.type == TransitionRoom.Type.shop)
                {
                    Destroy(room.gameObject);
                }
            }



            for (int i = 0; i < progression.compainData.levels.Length; i++)
            {
                if (progression.compainData.levels[i].levelAction == loadShop)
                {
                    progression.compainData.levels[i].levelAction = SkipTransition;
                }
            }
        }
    }
}