using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.SaveLoad
{
    [Serializable]
    public class GameOptionSaveData
    {
        public float soundVolume;
        public float musicVolume;

        public bool fullscreen;
    }
}
