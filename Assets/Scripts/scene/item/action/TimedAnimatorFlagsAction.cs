using net.gubbi.goofy.item;
using net.gubbi.goofy.player;
using net.gubbi.goofy.util;

namespace net.gubbi.goofy.scene.item.action {

    public class TimedAnimatorFlagsAction : SingleSceneItemAction {
        public string flag;
        public float time = Constants.DEFAULT_ACTION_TIME;
        public bool waitForAnimation;
        
        private TimedAnimatorFlag timedAnimatorFlag;

        protected override void Awake() {
            base.Awake();
            timedAnimatorFlag = GetComponentInParent<TimedAnimatorFlag>();
        }        

        protected override void doAction (ItemType selectedItem) {
            if (waitForAnimation) {
                timedAnimatorFlag.setFlag (afterAction, flag, time);
            }
            else {
                timedAnimatorFlag.setFlag (null, flag, time);
                afterAction();
            }
        }
    }
}
