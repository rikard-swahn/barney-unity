using net.gubbi.goofy.item;
using net.gubbi.goofy.scene.item.action;
using net.gubbi.goofy.state;
using net.gubbi.goofy.util;
using UnityEngine;

namespace Assets.Scripts.scene.item.action {
    public class SetAutosaveEnabledAction : SingleSceneItemAction {

        public new bool enabled;
        
        private PersistFacade _persistFacade;

        protected override void Awake() {
            base.Awake();
            _persistFacade = GameObject.FindGameObjectWithTag(Tags.ROOM).GetComponent<PersistFacade>();
        }

        protected override void doAction (ItemType selectedItem) {
            _persistFacade.setAutoSaveEnabled(enabled);
            afterAction();
        }

    }
}