using net.gubbi.goofy.character;
using UnityEngine;

namespace net.gubbi.goofy.filter {

    [CreateAssetMenu(menuName = "Filters/Character in current scene")]
    public class CharacterInCurrentScene : Filter {

        public bool requireInScene;

        public override bool matches(FilterContext ctx) {
            GameObject character = ctx.getProperty<GameObject>(FilterContext.CHARACTER);
            CharacterSceneHandler sceneHandler = character.GetComponentInChildren<CharacterSceneHandler> ();
            return sceneHandler.isCharacterInCurrentScene() == requireInScene;
        }
    }
}