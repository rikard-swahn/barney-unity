using net.gubbi.goofy.state;
using UnityEngine;

namespace net.gubbi.goofy.filter {

    [CreateAssetMenu(menuName = "Filters/Given character in given scene")]
    public class GivenCharacterInGivenScene : Filter {

        public string characterKey;
        public string scene;
        public bool requireInScene;

        public override bool matches(FilterContext ctx) {
            bool isInScene = scene.Equals(GameState.Instance.StateData.getCharacterScene(characterKey));
            
            return isInScene == requireInScene;
        }
    }
}