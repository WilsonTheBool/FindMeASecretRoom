using System.Collections;
using UnityEngine;
using Assets.Scripts.Game.PlayerController;

namespace Assets.Scripts.Game.GameDebug
{
    public class PLayerHP_DEBUG_Commands : MonoBehaviour
    {
        PlayerHPController playerController;

        public HpObject redHP;
        public HpObject blueHP;

        // Use this for initialization
        void Start()
        {
            playerController = Player.instance.playerHPController;
        }

        // Update is called once per frame
        void Update()
        {
            //if (Input.GetKeyUp(KeyCode.Alpha1))
            //{
            //    playerController.RequestTakeDamage(new PlayerHPController.HpEventArgs(1, null));
            //}

            //if (Input.GetKeyUp(KeyCode.Alpha2))
            //{
            //    playerController.RequestPickUpHeart(new PlayerHPController.HpEventArgs(1, redHP, null));
            //}

            //if (Input.GetKeyUp(KeyCode.Alpha3))
            //{
            //    playerController.RequestPickUpHeart(new PlayerHPController.HpEventArgs(1, blueHP, null));
            //}

            //if (Input.GetKeyUp(KeyCode.Alpha4))
            //{
            //    playerController.RequestAddContainer(new PlayerHPController.HpEventArgs(1, null));
            //}
        }
    }
}