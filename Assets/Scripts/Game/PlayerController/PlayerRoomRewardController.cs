using Assets.Scripts.Game.GameMap;
using Assets.Scripts.Game.Gameplay;
using Assets.Scripts.Game.UI;
using Assets.Scripts.LevelGeneration;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Game.PlayerController
{
    public class PlayerRoomRewardController : MonoBehaviour
    {
        [HideInInspector]
        public MainGameLevelMapController main;

        public PlayerGoldController gold;

        public int baseRoomReward;
        public int rewardIncreasePerCombo;

        public Vector3 goldAnimScaleStart;
        public Vector3 goldAnimScaleEnd;

        public int curentCombo;
        public int maxCombo;

        public RewardEvent OnReward;

        private GameUIController GameUIController;

        public UnityEvent onComboChanged;

        private void Start()
        {
            main = MainGameLevelMapController.Instance;

            main.GameMapRoomUnlockController.roomUnlocked.AddListener(OnRoomUnlocked);
            main.ExplosionController.onAfterExplosion.AddListener(OnExplosion);
            main.ExplosionController.onAfterExplosion_fake.AddListener(OnExplosion);
            main.onLevelOver.AddListener(ResetCombo);

            GameUIController = GameUIController.Instance;

            curentCombo = 1;
        }

        private void ResetCombo()
        {
            curentCombo = 1;
            onComboChanged.Invoke();
        }

        private void OnExplosion(Explosion explosion, ExplosionResult explosionResult)
        {
            if(explosionResult.secretRoomsUnlocked <= 0)
            {
                ResetCombo();
            }
        }

        private void OnRoomUnlocked(Room room)
        {
            if(room != null)

            if(room.type != null && room.type.isSecretRoom)
            {
                GiveRewards(room);

                curentCombo++;

                if(curentCombo > maxCombo)
                {
                    curentCombo = maxCombo;
                }

                onComboChanged.Invoke();
            }
        }

        private void GiveRewards(Room room)
        {
            int ammount = baseRoomReward + rewardIncreasePerCombo * (curentCombo - 1);

            var args = new RewardEventArgs(ammount, curentCombo);

            OnReward.Invoke(args);

            gold.AddGold(args.ammountChanged);

            GameUIController.CreateTransferAnimation(new GameUIController.TransferAnimData
            {
                origin = main.grid.GetCellCenter(room.position),
                destination = GameUIController.GetPosition_Ui_To_World(GameUIController.playerGoldUIController.transform.position),
                prefab = GameUIController.gold_prefab,
                startScale = goldAnimScaleStart,
                endScale = goldAnimScaleEnd,
            }
            , args.ammount);
        }

        [System.Serializable]
       public class RewardEvent: UnityEvent<RewardEventArgs>
        {

        }

        public class RewardEventArgs
        {
            public readonly int ammount;

            public int ammountChanged;

            public int curentCombo;

            public RewardEventArgs(int ammount, int combo)
            {
                this.ammount = ammount;
                this.ammountChanged = ammount;
                curentCombo = combo;
            }
        }
    }
}