using net.gubbi.goofy.state;
using UnityEngine;

namespace net.gubbi.goofy.filter {

    [CreateAssetMenu(menuName = "Filters/State flags")]
    public class StateFlagsSetFilter : Filter {

        public string[] flags;
        public bool checkSet = true;

        public override bool matches(FilterContext ctx) {
            if (checkSet) {
                return GameState.Instance.StateData.flagsSet(flags);
            }

            return GameState.Instance.StateData.flagsNotSet(flags);
        }
    }
}