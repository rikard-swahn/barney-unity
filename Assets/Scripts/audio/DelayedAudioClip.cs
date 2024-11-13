using System;
using UnityEngine;

namespace net.gubbi.goofy.audio {
    
    [Serializable]
    public class DelayedAudioClip {
        public AudioClip clip;
        public float delay;
        public bool loop;
    }
}