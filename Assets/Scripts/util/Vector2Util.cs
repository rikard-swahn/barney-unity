using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace net.gubbi.goofy.util {
    public class Vector2Util {

        public static int? looseIndexOf(IEnumerable<Vector2> enumerable, Vector2 v1) {
            
            for(int i = 0; i < enumerable.Count(); i++) {                
                if (v1 == enumerable.ElementAt(i)) {
                    return i;
                }
            }

            return null;
        }
     
    }
}