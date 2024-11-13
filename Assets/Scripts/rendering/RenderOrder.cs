using net.gubbi.goofy.util;
using UnityEngine;

namespace net.gubbi.goofy.scene {
    public class RenderOrder : MonoBehaviour {

        private SpriteRenderer spriteRenderer;
        private Transform selfBaseline;

        private void Awake() {
            spriteRenderer = GetComponent<SpriteRenderer>();
            selfBaseline = transform.Find (GameObjects.BASE_LINE);
        }

        private void Update() {
            spriteRenderer.sortingOrder = -(int)(selfBaseline.position.y * 1000);
        }

    }
}