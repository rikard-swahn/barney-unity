using net.gubbi.goofy.character;
using UnityEngine;

namespace net.gubbi.goofy.filter {

    [CreateAssetMenu(menuName = "Filters/Character in scene")]
    public class CharacterInScene : Filter {

        public string scene;

        public override bool matches(FilterContext ctx) {
            GameObject character = ctx.getProperty<GameObject>(FilterContext.CHARACTER);
            CharacterSceneHandler sceneHandler = character.GetComponentInChildren<CharacterSceneHandler> ();
            return sceneHandler.isCharacterInScene(scene);
        }
    }
}