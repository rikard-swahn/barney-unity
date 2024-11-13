using net.gubbi.goofy.item;
using net.gubbi.goofy.scene.item.action;
using net.gubbi.goofy.state;

namespace Assets.Scripts.scene.item.action {
    public class CompleteGoalAction : SingleSceneItemAction {
        public int goal;

        protected override void doAction(ItemType selectedItem) {
            GameState.Instance.StateData.setGoalComplete(goal);
            afterAction();
        }
    }
}