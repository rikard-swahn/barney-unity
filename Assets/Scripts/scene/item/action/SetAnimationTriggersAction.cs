using System.Collections.Generic;
using net.gubbi.goofy.extensions;
using net.gubbi.goofy.item;
using net.gubbi.goofy.scene.item.action;
using UnityEngine;

namespace Assets.Scripts.scene.item.action {
    public class SetAnimationTriggersAction : SingleSceneItemAction {

        public List<string> triggers;

        private Animator[] targetAnimators;

        protected override void Awake() {
            base.Awake ();
            targetAnimators = target.GetComponentsInChildren<Animator>();
        }

        protected override void doAction(ItemType selectedItem) {
            triggers.ForEach(t => {               
                targetAnimators.ForEach(a => a.SetTrigger(t));
            });
            afterAction();            
        }
    }
}