using System.Collections.Generic;
using net.gubbi.goofy.util;
using UnityEngine;

namespace net.gubbi.goofy.scene.load.transition {
    public class Transitions {

        private static readonly Dictionary<Type, SceneTransition> transitions = new Dictionary<Type, SceneTransition>();
        private static readonly float TRANSITION_TIME = 1f;
        private static readonly float TRANSITION_TIME_SLOW = 5f;
        
        public enum Type {
            EMPTY, 
            CIRCLE_OUT, CIRCLE_IN,
            BOX_OUT, BOX_IN,
            FADE_IN, FADE_OUT,
            HEART_OUT,
            CIRCLE_OUT_NO_MUSIC_FADE
        }

        static Transitions() {
            transitions.Add(Type.CIRCLE_OUT, CircleTransition.create<CircleOutTransition>(Textures.hole(), Color.black, TRANSITION_TIME));
            transitions.Add(Type.CIRCLE_OUT_NO_MUSIC_FADE, CircleTransition.create<CircleOutTransition>(Textures.hole(), Color.black, TRANSITION_TIME, false));
            transitions.Add(Type.CIRCLE_IN, CircleTransition.create<CircleInTransition>(Textures.hole(), Color.black, TRANSITION_TIME));
            transitions.Add(Type.HEART_OUT, CircleTransition.create<CircleOutTransition>(Textures.heart(), Color.black, TRANSITION_TIME_SLOW));
            transitions.Add(Type.BOX_OUT, BoxOutTransition.create(Color.black, TRANSITION_TIME));
            transitions.Add(Type.BOX_IN, BoxInTransition.create(Color.black, TRANSITION_TIME));
            transitions.Add(Type.FADE_OUT, FadeOutTransition.create(Color.black, TRANSITION_TIME));
            transitions.Add(Type.FADE_IN, FadeInTransition.create(Color.black, TRANSITION_TIME));            
        }

        public static SceneTransition get(Type type) {
            if (type == Type.EMPTY || !transitions.ContainsKey(type)) {
                return null;        
            }
            
            return transitions[type];
        }
                
    }
}