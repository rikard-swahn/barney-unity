using System;
using System.Collections;
using net.gubbi.goofy.item;
using UnityEngine;

namespace net.gubbi.goofy.scene.item.action {
    public class WaitAction : SingleSceneItemAction {

        public float waitTime = 1.0f;

        protected override void doAction (ItemType selectedItem) {
            StartCoroutine(wait (afterAction));
        }        
        
        private IEnumerator wait (Action afterWait) {
            yield return new WaitForSeconds(waitTime);
            afterWait ();
        }        
			
    }
}