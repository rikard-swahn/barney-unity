using net.gubbi.goofy.character;
using UnityEngine;

namespace net.gubbi.goofy.filter {

    [CreateAssetMenu(menuName = "Filters/Character direction")]
    public class CharacterDirectionFilter : Filter {

        public Vector2 direction;

        public override bool matches(FilterContext ctx) {
            GameObject character = ctx.getProperty<GameObject>(FilterContext.CHARACTER);
            CharacterFacade characterFacade = character.GetComponent<CharacterFacade>();
            return direction.Equals(characterFacade.getNormalizedDirection());
        }
    }
}