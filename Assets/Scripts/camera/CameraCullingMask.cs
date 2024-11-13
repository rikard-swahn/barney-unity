using System.Collections.Generic;
using UnityEngine;

namespace net.gubbi.goofy.camera {
    public class CameraCullingMask : MonoBehaviour {

        public List<string> Layers;
        public bool Include;
        
        private void Awake() {
            Camera cam = GetComponent<Camera>();
            
            if (Include) {
                cam.cullingMask = 0;
                Layers.ForEach(l => cam.cullingMask = 1 << LayerMask.NameToLayer(l));
            }
            else {
                Layers.ForEach(l => cam.cullingMask = cam.cullingMask & ~(1 << LayerMask.NameToLayer(l)));
            }
        }        

    }
}