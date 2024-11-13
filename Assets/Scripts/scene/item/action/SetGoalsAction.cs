using System.Collections.Generic;
using net.gubbi.goofy.events;
using net.gubbi.goofy.item;
using net.gubbi.goofy.scene.item.action;
using net.gubbi.goofy.state;

namespace Assets.Scripts.scene.item.action {
    public class SetGoalsAction : SingleSceneItemAction {
        public List<string> goals;

        protected override void doAction(ItemType selectedItem) {            
            GameState.Instance.StateData.setGoals(goals);
            EventManager.Instance.raise(new GameEvents.GoalsSetEvent());
            afterAction();
        }
    }
}