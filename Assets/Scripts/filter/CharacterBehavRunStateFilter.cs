using net.gubbi.goofy.character.npc.behaviour.state;
using net.gubbi.goofy.state;
using UnityEngine;

namespace net.gubbi.goofy.filter {

    [CreateAssetMenu(menuName = "Filters/Character run state empty")]
    public class CharacterBehavRunStateFilter : Filter {

        public string characterKey;

        public override bool matches(FilterContext ctx) {
            BehaviourRunStateType? state = GameState.Instance.StateData.getCharacterBehaviourRunState (characterKey);
            return state == null;
        }
    }
}