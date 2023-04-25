using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Game.PlayerController
{
    public class PlayerHPController : MonoBehaviour
    {
        [SerializeField]
        public int maxHpSlotsCount;

        [SerializeField]
        int startHp;

        public List<HpObject> hpObjects;

        public HpObject redHelath;

        [SerializeField]
        HpControllerRule rule;

        [HideInInspector]
        public HpControllerEvent beforeCntainerAdd;
        public HpControllerEvent afterCntainerAdd;

        [HideInInspector]
        public HpControllerEvent beforeTakeDamage;
        public HpControllerEvent afterTakeDamage;

        [HideInInspector]
        public HpControllerEvent beforePickUpHeart;
        public HpControllerEvent afterPickUpHeart;

        [HideInInspector]
        public PlayerDeathEvent beforeDeath;
        public PlayerDeathEvent afterDeath;

        public HpControllerEvent onAnyHpChanged;
        private void Awake()
        {
            Initialize(true);
        }

        public void RequestTakeDamage(HpEventArgs args)
        {
            beforeTakeDamage.Invoke(args);

            TakeDamage(args);
        }

        private void TakeDamage(HpEventArgs args)
        {
            if(args.change <= 0)
            {
                return;
            }

            rule.OnTakeDamage(hpObjects, args.change);

            afterTakeDamage.Invoke(args);

            onAnyHpChanged.Invoke(args);

            if (rule.IsDead(hpObjects))
            {
                RequestDeath(new DeathEventArgs(true));
            }

            
        }

        public bool CanPickUpHeart(HpObject heart)
        {
            if(heart == null)
            {
                return false;
            }
            
            return rule.CanPickUpHeart(hpObjects, heart, maxHpSlotsCount);
        }

        public void HealRedHP(int count)
        {
            for(int i = 0; i < count; i++)
            {
                if (CanPickUpHeart(redHelath))
                {
                    RequestPickUpHeart(new HpEventArgs(1, redHelath, null));
                }
                else
                {
                    break;
                }
            }
        }

        public void HealToFullRedHP()
        {
            while(CanPickUpHeart(redHelath ))
            {
                PickUpHeart(new HpEventArgs(1, redHelath, null));
            }
        }

        public void RequestPickUpHeart(HpEventArgs args)
        {
            if (CanPickUpHeart(args.HpObject))
            {

                beforePickUpHeart.Invoke(args);

                PickUpHeart(args);
            }

        }

        private void PickUpHeart(HpEventArgs args)
        {
            for (int i = 0; i < args.change; i++)
                rule.OnPickUpHeart(hpObjects, args.HpObject);

            afterPickUpHeart.Invoke(args);

            onAnyHpChanged.Invoke(args);
        }

        public void RequestDeath(DeathEventArgs args)
        {
            beforeDeath.Invoke(args);

            if (args.isDeadChanged)
            {
                Die(args);
            }
        }

        private void Die(DeathEventArgs args)
        {
            afterDeath.Invoke(args);
        }

        public void RequestAddContainer(HpEventArgs args)
        {
            beforeCntainerAdd.Invoke(args);

            AddContainer(args);
        }

        private void AddContainer(HpEventArgs args)
        {
            for(int i = 0; i < args.change; i++)
            {
                rule.OnAddContainer(hpObjects, maxHpSlotsCount);
            }
            

            afterCntainerAdd.Invoke(args);

            onAnyHpChanged.Invoke(args);
        }

        public void RemoveContainer(HpEventArgs args)
        {
            for (int i = 0; i < args.change; i++)
            {
                rule.OnRemoveContainer(hpObjects);
            }
            onAnyHpChanged.Invoke(args);
        }

        public class HpEventArgs
        {
            public readonly int originalChange;
            public int change;

            public HpObject HpObject;

            public readonly GameObject changeHpSource;

            public HpEventArgs(int originalChange, HpObject hpObject, GameObject changeHpSource)
            {
                this.originalChange = originalChange;
                change = this.originalChange;
                this.changeHpSource = changeHpSource;
                HpObject = hpObject;
            }

            public HpEventArgs(int originalChange, GameObject changeHpSource)
            {
                this.originalChange = originalChange;
                change = this.originalChange;
                this.changeHpSource = changeHpSource;
                HpObject = null;
            }
        }

        public class DeathEventArgs
        {
            public readonly bool isDead;

            public bool isDeadChanged;

            public DeathEventArgs(bool isDead)
            {
                this.isDead = isDead;
                isDeadChanged = isDead;
            }
        }

        public void SetStartHP(int red)
        {
            hpObjects.Clear();

            for (int i = 0; i < red; i++)
            {
                rule.OnAddContainer(hpObjects, maxHpSlotsCount);
            }

            onAnyHpChanged.Invoke(new HpEventArgs(red, null));
        }

        public void Initialize(bool fromStart)
        {
            if (fromStart)
            {
                AddContainer(new HpEventArgs(startHp, null));
            }
        }


        [System.Serializable]
        public class HpControllerEvent : UnityEvent<HpEventArgs>{ }


        [System.Serializable]
        public class PlayerDeathEvent : UnityEvent<DeathEventArgs> { }
    }


}