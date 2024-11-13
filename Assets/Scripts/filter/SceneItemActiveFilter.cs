using net.gubbi.goofy.extensions;
using net.gubbi.goofy.scene.item;
using UnityEngine;

namespace net.gubbi.goofy.filter {

    [CreateAssetMenu(menuName = "Filters/Scene item active")]
    public class SceneItemActiveFilter : Filter {

        public bool active = true;
        public string itemGameObject;

        public override bool matches(FilterContext ctx) {
            SceneItem sceneItem = itemGameObject.isNullOrEmpty() ? ctx.getProperty<SceneItem>(FilterContext.SCENE_ITEM) : GameObject.Find(itemGameObject).GetComponent<SceneItem>();
            return sceneItem.isActive() == active;
        }
    }
}