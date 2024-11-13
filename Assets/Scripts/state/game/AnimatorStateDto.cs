using System.Collections.Generic;
using System.Linq;

namespace net.gubbi.goofy.state.game {
    
    public class AnimatorStateDto {
        public List<string> boolParams;

        public AnimatorStateDto() { }

        public AnimatorStateDto(AnimatorState state) {
            boolParams = state.BoolParams.ToList();
        }        
    }
}