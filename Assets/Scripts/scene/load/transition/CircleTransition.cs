using System;
using net.gubbi.goofy.events;
using UnityEngine;

namespace net.gubbi.goofy.scene.load.transition {
        
    public abstract class CircleTransition : SceneTransition {
        
        public Color color = Color.black;
        public Texture2D circleTexture;
        private Color fadeColor;
        private Texture2D borderTexture;
        private Vector2 center = Vector2.zero;
        private static readonly float fadeFactor = 0.25f;

        private void Awake() {
            fadeColor = color;
        }

        private void handlePlayerRenderPositionEvent(GameEvents.PlayerRenderPositionEvent e) {
            center = e.Position;
        }

        protected override void doOnGUI() {
            GUI.color = color;
            
            Vector3 center = this.center != Vector2.zero ? Camera.main.WorldToScreenPoint(this.center) : new Vector3(Screen.width / 2f, Screen.height / 2f);
            center = new Vector2(center.x, Screen.height - center.y);
            
            float r1 = Mathf.Sqrt(Mathf.Pow(center.x, 2f) + Mathf.Pow(center.y, 2f));
            float r2 = Mathf.Sqrt(Mathf.Pow(center.x, 2f) + Mathf.Pow(Screen.height - center.y, 2f));
            float r3 = Mathf.Sqrt(Mathf.Pow(Screen.width - center.x, 2f) + Mathf.Pow(Screen.height - center.y, 2f));
            float r4 = Mathf.Sqrt(Mathf.Pow(Screen.width - center.x, 2f) + Mathf.Pow(Screen.height, 2f));
            float maxRadius = Mathf.Max(r1, r2, r3, r4);
            float r = maxRadius * getScale(progress);
            float d = 2 * r;            
            
            float bottom = center.y + r;
            float top = center.y - r;            
            float left = center.x - r;
            float right = center.x + r;            
            GUI.DrawTexture(new Rect(left, top, d, d), circleTexture);
            
            //Make the rectangles overlap the circle sprite, to avoid gaps
            float overlapFactor = 0.015f;
            float overlap = d * overlapFactor;

            GUI.DrawTexture(new Rect(0, 0, left + overlap, Screen.height), borderTexture);
            GUI.DrawTexture(new Rect(right - overlap, 0, Screen.width - right + overlap * 2, Screen.height), borderTexture);
            GUI.DrawTexture(new Rect(0, 0, Screen.width, top + overlap), borderTexture);
            GUI.DrawTexture(new Rect(0, bottom - overlap, Screen.width, Screen.height - bottom + overlap * 2), borderTexture);

            fadeColor.a = fadeFactor - getScale(progress) * fadeFactor;
            GUI.color = fadeColor;
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), borderTexture);            
            
            this.center = Vector2.zero;
        }

        protected abstract float getScale(float progress);

        public override bool start(Action onComplete) {
            if (!base.start(onComplete)) {
                return false;
            }
            
            EventManager.Instance.addListener<GameEvents.PlayerRenderPositionEvent>(handlePlayerRenderPositionEvent);
            center = Vector2.zero;
            
            if (borderTexture == null) {
                borderTexture = new Texture2D(1, 1);
                borderTexture.SetPixel(0, 0, Color.white);
                borderTexture.Apply();
            }

            return true;
        }

        public override void stop() {
            base.stop();
            EventManager.Instance.removeListener<GameEvents.PlayerRenderPositionEvent>(handlePlayerRenderPositionEvent);
        }

        public static CircleTransition create<T>(Texture2D texture, Color? color = null, float? transitionTime = null, bool fadeMusic = true) where T : CircleTransition {
            T transition = CreateInstance<T>();
            transition.circleTexture = texture;
            if (color != null) {
                transition.color = (Color)color;                
            }
            if (transitionTime != null) {
                transition.transitionTime = (float)transitionTime;
            }
            transition.fadeMusic = fadeMusic;
            return transition;
        }       

    }
}