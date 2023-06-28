using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.SaveLoad
{
    public interface ISaveLoadComponent
    {
        public abstract int GetOrder();

        public abstract void SetUp(SaveLoadController controller);

        public void OnAwake(SaveLoadController controller);

        public   void OnAfterAwake(SaveLoadController controller);

        public   void OnStart(SaveLoadController controller);

        public void Load_SaveData(SaveLoadController controller);
    }
}
