using net.gubbi.goofy.character;
using UnityEngine;

namespace net.gubbi.goofy.filter {

    [CreateAssetMenu(menuName = "Filters/Given character in current scene")]
    public class GivenCharacterInCurrentScene : Filter {

        public string characterKey;
        public bool requireInScene;

        public override bool matches(FilterContext ctx) {
            GameObject character = GameObject.Find(characterKey);
            if (character == null) {
                return !requireInScene;
            }

            CharacterSceneHandler sceneHandler = character.GetComponentInChildren<CharacterSceneHandler> ();
            return sceneHandler.isCharacterInCurrentScene() == requireInScene;
        }
    }
}