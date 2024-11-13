using net.gubbi.goofy.scene.item;
using net.gubbi.goofy.state;
using net.gubbi.goofy.util;
using UnityEngine;

namespace net.gubbi.goofy.scene.scenes.map {

    //TODO: Make some general component
    public class ReturnGrandma : MonoBehaviour {

        private void Awake() {
            string grandmaScene = GameState.Instance.StateData.getCharacterScene(Characters.GRANDMA_DUCK);

            if (Scenes.GOOFY_DOWNSTAIRS.Equals(grandmaScene)) {
                GameState.Instance.StateData.changeCharacterScene (Characters.GRANDMA_DUCK, Scenes.GRANDMA_HOUSE);
                GameState.Instance.StateData.setFlags (Flags.GRANDMA_ABANDONED);
                
                SceneItemUtil.setItemActive(GameObjects.COFFEE_CUP, false);
                GameState.Instance.StateData.setFlags (Flags.GRANDMA_ABANDONED);
            }
        }

    }

}