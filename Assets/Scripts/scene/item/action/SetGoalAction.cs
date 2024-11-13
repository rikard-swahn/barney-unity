using net.gubbi.goofy.item;
using net.gubbi.goofy.scene.item.action;
using net.gubbi.goofy.state;

namespace Assets.Scripts.scene.item.action {
    public class SetGoalAction : SingleSceneItemAction {
        public int index;
        public string goal;

        protected override void doAction(ItemType selectedItem) {            
            GameState.Instance.StateData.setGoal(index, goal);
            afterAction();
        }
    }
}