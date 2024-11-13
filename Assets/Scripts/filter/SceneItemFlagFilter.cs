using net.gubbi.goofy.state;
using UnityEngine;

namespace net.gubbi.goofy.filter {

    [CreateAssetMenu(menuName = "Filters/Scene item flag")]
    public class SceneItemFlagFilter : Filter {

        public string itemKey;
        public string itemProperty;
        public bool checkSet = true;

        public override bool matches(FilterContext ctx) {
            bool hasProperty = GameState.Instance.StateData.hasSceneProperty(itemKey, itemProperty);

            return hasProperty && checkSet == GameState.Instance.StateData.getSceneProperty(itemKey, itemProperty).getBool()
                   || !hasProperty && checkSet == false;
        }
    }
}