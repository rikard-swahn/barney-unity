using System.Collections.Generic;
using net.gubbi.goofy.extensions;
using net.gubbi.goofy.item;
using net.gubbi.goofy.util;
using UnityEngine;

namespace net.gubbi.goofy.scene.item.action {

    public class SetAnimatorFlagsAction : SingleSceneItemAction {
        public List<string> flags;
        public bool set;
        
        private Animator[] animators;
        
        protected override void Awake() {
            base.Awake();
            
            GameObject playerGo = GameObject.Find(GameObjects.PLAYER);
            animators = playerGo.GetComponentsInChildren<Animator>();            
        }

        protected override void doAction (ItemType selectedItem) {
            flags.ForEach(f => {
                animators.ForEach(a => a.SetBool(f, set));
            });
            afterAction();
        }
    }
}
