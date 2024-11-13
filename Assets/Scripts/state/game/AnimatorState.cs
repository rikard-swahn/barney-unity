using System.Collections.Generic;

namespace net.gubbi.goofy.state.game {
    public class AnimatorState {
        public HashSet<string> BoolParams {get; set;}

        public AnimatorState() {
            BoolParams = new HashSet<string>();
        }

        public AnimatorState(AnimatorStateDto dto) {
            BoolParams = new HashSet<string>(dto.boolParams);
        }
    }
}