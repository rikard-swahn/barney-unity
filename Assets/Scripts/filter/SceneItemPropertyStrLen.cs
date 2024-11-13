using net.gubbi.goofy.state;
using UnityEngine;

namespace net.gubbi.goofy.filter {

    [CreateAssetMenu(menuName = "Filters/Scene item property string length equals")]
    public class SceneItemPropertyStrLen : Filter {

        public string itemKey;
        public string itemProperty;
        public int length;

        public override bool matches(FilterContext ctx) {
            string val = GameState.Instance.StateData.getSceneProperty(itemKey, itemProperty).stringVal;
            
            return GameState.Instance.StateData.hasSceneProperty(itemKey, itemProperty) &&
                   val != null && val.Length == length;
        }
    }
}