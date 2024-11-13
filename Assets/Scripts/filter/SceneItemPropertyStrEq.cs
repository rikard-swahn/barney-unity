using net.gubbi.goofy.state;
using UnityEngine;

namespace net.gubbi.goofy.filter {

    [CreateAssetMenu(menuName = "Filters/Scene item property string equals")]
    public class SceneItemPropertyStrEq : Filter {

        public string itemKey;
        public string itemProperty;
        public string compareTo;

        public override bool matches(FilterContext ctx) {
            return GameState.Instance.StateData.hasSceneProperty(itemKey, itemProperty) &&
                   compareTo.Equals(GameState.Instance.StateData.getSceneProperty(itemKey, itemProperty).stringVal);
        }
    }
}