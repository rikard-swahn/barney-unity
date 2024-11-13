using System.Collections.Generic;
using UnityEngine;

namespace net.gubbi.goofy.scene {
    public class ScrollParallax : MonoBehaviour {

        public GameObject ScrollPrefab;
        public Camera Camera;
        public float ParallaxFactor = 1.0f;

        private List<GameObject> _scrolledObjects = new List<GameObject>();
        private SpriteRenderer _renderer;
        private GameObject _left;
        private GameObject _center;
        private GameObject _right;
        private Vector3 _xOffset;
        private float _width;
        private Vector3 _lastCamPos;

        private void Awake() {
            _renderer = ScrollPrefab.GetComponent<SpriteRenderer>();
            _width = _renderer.bounds.size.x;
            _xOffset = Vector3.right * _width;

            if (Camera == null) {
                Camera = Camera.main;
            }
        }
        
        private void LateUpdate() {
            Parallax();
            Scroll();
        }

        private void Parallax() {
            float xDelta = Camera.transform.position.x - _lastCamPos.x;
            transform.Translate(Vector3.right * xDelta * ParallaxFactor);
            _lastCamPos = new Vector3(Camera.transform.position.x, Camera.transform.position.y, Camera.transform.position.z);            
        }

        private void Scroll() {
            float screenAspect = (float) Screen.width / (float) Screen.height;
            float cameraHeight = Camera.orthographicSize * 2;
            float cameraWidth = cameraHeight * screenAspect;
            float cameraXMin = Camera.transform.position.x - cameraWidth / 2;
            float cameraXMax = Camera.transform.position.x + cameraWidth / 2;            

            int expectedScrollObjCount = (int) Mathf.Ceil(cameraWidth / _width) + 2;
            if (_scrolledObjects.Count != expectedScrollObjCount) {
                _scrolledObjects.ForEach(Destroy);
                _scrolledObjects = new List<GameObject>();

                Vector3 leftPos = new Vector3(
                    cameraXMin - (1 - _renderer.sprite.pivot.x) * _width, 
                    transform.position.y, 
                    transform.position.z);
                
                for (int i = 0; i < expectedScrollObjCount; i++) {
                    GameObject o = Instantiate(ScrollPrefab, leftPos + _xOffset * i, Quaternion.identity);
                    o.transform.parent = gameObject.transform;
                    _scrolledObjects.Add(o);
                }
            }

            while (GetXMax(_scrolledObjects.Count - 1) < cameraXMax) {
                GameObject left = _scrolledObjects[0];
                left.transform.Translate(Vector3.right * GetScrollObjectsWidth());
                _scrolledObjects.Remove(left);
                _scrolledObjects.Add(left);
            }

            while (GetXMin(0) > cameraXMin) {
                GameObject right = _scrolledObjects[_scrolledObjects.Count - 1];
                right.transform.Translate(Vector3.left * GetScrollObjectsWidth());
                _scrolledObjects.Remove(right);
                _scrolledObjects.Insert(0, right);
            }
        }

        private float GetXMin(int i) {
            return _scrolledObjects[i].GetComponent<SpriteRenderer>().bounds.min.x;
        }
        private float GetXMax(int i) {
            return _scrolledObjects[i].GetComponent<SpriteRenderer>().bounds.max.x;
        }        
        private float GetScrollObjectsWidth() {
            return _scrolledObjects.Count * _width;
        }
    }
}