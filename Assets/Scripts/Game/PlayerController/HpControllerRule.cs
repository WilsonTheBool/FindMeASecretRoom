using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game.PlayerController
{

    public class HpControllerRule : MonoBehaviour
    { 
        [SerializeField]
        HpObject red;
        [SerializeField]
        HpObject empty;
        [SerializeField]
        HpObject blue;

        HpObjectComparer comparer;

        private void Awake()
        {
            comparer = new HpObjectComparer();
        }

        public void OnAddContainer(List<HpObject> hpObjects, int maxCount)
        {
            if(hpObjects.Count < maxCount)
            {
                hpObjects.Add(red);

                hpObjects.Sort(comparer);
            }
            else
            {
                if (hpObjects.Contains(blue))
                {
                    hpObjects.Remove(blue);
                    hpObjects.Add(red);

                    hpObjects.Sort(comparer);
                }
            }
        }

        public void OnAddContainer_Empty(List<HpObject> hpObjects, int maxCount)
        {
            if (hpObjects.Count < maxCount)
            {
                hpObjects.Add(empty);

                hpObjects.Sort(comparer);
            }
            else
            {
                if (hpObjects.Contains(blue))
                {
                    hpObjects.Remove(blue);
                    hpObjects.Add(empty);

                    hpObjects.Sort(comparer);
                }
            }
        }

        public void OnRemoveContainer(List<HpObject> hpObjects)
        {
            if (hpObjects.Contains(empty))
            {
                hpObjects.Remove(empty);
            }
            else
            {
                if (hpObjects.Contains(red))
                {
                    hpObjects.Remove(red);
                }
            }

            hpObjects.Sort(comparer);
        }

        public void OnTakeDamage(List<HpObject> hpObjects, int count)
        {
            for (int i = 0; i < count; i++)
            {
                if (hpObjects.Contains(blue))
                {
                    hpObjects.Remove(blue);
                }
                else
                {
                    if (hpObjects.Contains(red))
                    {
                        hpObjects.Remove(red);
                        hpObjects.Add(empty);

                        hpObjects.Sort(comparer);
                    }
                }
            }
        }

        public bool CanPickUpHeart(List<HpObject> hpObjects, HpObject heartType, int maxCount)
        {
            if(heartType.type == HpType.red)
            {
                return hpObjects.Contains(empty);
            }
            
            if(heartType.type == HpType.blue)
            {
                return hpObjects.Count <= maxCount;
            }

            return false;
        }

        public void OnPickUpHeart(List<HpObject> hpObjects, HpObject heartType)
        {
            if(heartType.type == HpType.red)
            {
                hpObjects.Remove(empty);
                hpObjects.Add(red);

                hpObjects.Sort(comparer);

                return;
            }

            if(heartType.type == HpType.blue)
            {
                hpObjects.Add(blue);

                hpObjects.Sort(comparer);

                return;
            }

        }

        public int GetHeartCountOfType(List<HpObject> hpObjects, HpObject heartType)
        {
            int count = 0;

            foreach(HpObject hpObject in hpObjects)
            {
                if(hpObject == heartType)
                {
                    count++;
                }
            }

            return count;
        }

        public bool IsDead(List<HpObject> hpObjects)
        {
            if (hpObjects.Contains(red) || hpObjects.Contains(blue))
            {
                return false;
            }

            return true;
        }

        private class HpObjectComparer : Comparer<HpObject>
        {
            public override int Compare(HpObject x, HpObject y)
            {
                return x.type.CompareTo(y.type);
            }
        }
    }
}