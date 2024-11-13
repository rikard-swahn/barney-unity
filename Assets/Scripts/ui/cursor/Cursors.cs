using System.Collections.Generic;
using net.gubbi.goofy.events;
using net.gubbi.goofy.util;
using UnityEngine;

namespace net.gubbi.goofy.ui.cursor {

    public class Cursors : MonoBehaviour {

        public List<CustomCursor> cursors;
        public CursorType defaultCursor;
        public float cursorScale;
        public bool lockToDefault;
        
        private Dictionary<CursorType, CustomCursor> cursorMap = new Dictionary<CursorType, CustomCursor>();
        private float time;
        private CursorType currentCursorType;
        private Texture2D currentTexture;
        private Vector2 cursorSize;
        private ScaleProvider scaleProvider;

        private void Awake() {
            scaleProvider = GameObject.FindGameObjectWithTag(Tags.MAIN_CANVAS).GetComponentInParent<ScaleProvider>();
            
            foreach (CustomCursor cursor in cursors) {
                addCursor(cursor);
            }

            if (!lockToDefault) {
                EventManager.Instance.addListener(delegate(GameEvents.CursorEvent e) { SetCursor(e.Type); });
            }

            Cursor.lockState = CursorLockMode.Confined;
            SetCursor(defaultCursor);
        }

        private void addCursor(CustomCursor cursor) {
            cursorMap[cursor.type] = cursor;
        }

        private void Update() {
            var cursor = getCurrentCursor();
            float totalScale = scaleProvider.getScale() * cursorScale;
            Texture2D texture = getCursorTexture(cursor, totalScale);

            if (texture != currentTexture) {    
                currentTexture = texture;
                Cursor.SetCursor(texture, cursor.hotspot * totalScale, CursorMode.ForceSoftware);
            }
        }

        private Texture2D getCursorTexture(CustomCursor cursor, float scale) {
            if (cursor.textures.Count == 1) {
                return cursor.getScaledTextures(scale)[0];
            }
            
            time += Time.deltaTime;
            time = time % cursor.period;
            int tIndex = Mathf.FloorToInt(time * cursor.frameRate);
            return cursor.getScaledTextures(scale)[tIndex];
        }

        private void SetCursor(CursorType type) {
            if (currentCursorType == type) {
                return;
            }
            
            currentCursorType = type;
            time = 0f;            
        }

        private CustomCursor getCurrentCursor() {
            return cursorMap[currentCursorType];
        }
    }

}