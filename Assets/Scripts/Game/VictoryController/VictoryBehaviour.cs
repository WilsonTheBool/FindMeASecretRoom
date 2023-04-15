using Assets.Scripts.Game.GameMap;

namespace Assets.Scripts.Game.VictoryController
{
    public abstract class VictoryBehaviour
    {
        public void InvokeVictory()
        {
            GameVictoryController.InvokeVictory();
        }

        public abstract void OnAdd();
        public abstract void OnRemove();
    }

    public class Normal_VictoryBhaviour: VictoryBehaviour
    {
        private GameRoomCounter _counter;
        public Normal_VictoryBhaviour(GameRoomCounter counter)
        {
            _counter = counter;
        }

        public override void OnAdd()
        {

            if (_counter != null)
            {
                _counter.onVictory.AddListener(InvokeVictory);
            }
        }

        public override void OnRemove()
        {

            if (_counter != null)
            {
                _counter.onVictory.RemoveListener(InvokeVictory);
            }
        }
    }
}