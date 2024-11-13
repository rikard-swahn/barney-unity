using System;
using System.Collections.Generic;
using net.gubbi.goofy.filter;
using net.gubbi.goofy.item;
using UnityEngine;

namespace scene.item.action {
    public abstract class SceneItemAction : MonoBehaviour {
        public List<Filter> conditions;
        private static readonly string CONTROL_BY_ID = "action";

        public abstract bool filter(ItemType selectedItem);        
        public abstract void start(ItemType selectedItem, Action onComplete);
        public abstract void resume(ItemType selectedItem, Action onComplete);

        public virtual string getLabelPrefix() {
            return null;
        }

        protected string getControlById() {
            return CONTROL_BY_ID + "-" + GetInstanceID();
        }
    }
}