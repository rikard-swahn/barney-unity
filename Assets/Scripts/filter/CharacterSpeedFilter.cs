using net.gubbi.goofy.character;
using net.gubbi.goofy.extensions;
using UnityEngine;

namespace net.gubbi.goofy.filter {

    [CreateAssetMenu(menuName = "Filters/Character speed")]
    public class CharacterSpeedFilter : Filter {

        public float speed;

        public override bool matches(FilterContext ctx) {
            GameObject character = ctx.getProperty<GameObject>(FilterContext.CHARACTER);
            CharacterMove move = character.GetComponent<CharacterMove>();
            return speed.looseEquals(move.getSpeed(), 0.01f);
        }
    }
}