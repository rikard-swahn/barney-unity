using System.Collections;
using net.gubbi.goofy.extensions;
using net.gubbi.goofy.util;
using UnityEngine;

namespace net.gubbi.goofy.scene {
    public class ScrollTeleport : MonoBehaviour {

        public VectorRange velocity;
        public Range waitBeforeRestart;
        public Rect allowedRect;        
        public Rect spawnRect;        
        [HideInInspector]
        public Vector2 ScreenPivot = new Vector2(0.5f, 0.5f);
        
        private Rect allowedRectAdjusted;
        private Rect spawnRectAdjusted;
        private Rigidbody2D rigidBody;        
        private new SpriteRenderer renderer;
        private Vector2 relativePivot;
        private bool initiated;
                
        private void Awake() {
            renderer = GetComponent<SpriteRenderer>();
            relativePivot = new Vector2(0.5f, 0.5f);   //Only 0.5 pivot supported now. found no way to get pivot when sprite in sprite atlas. need sprite size to get relative pivot, but only atlas texture size available?       
            rigidBody = GetComponent<Rigidbody2D>();            
        }

        private void Update() {
            Rect thisRect = new Rect(renderer.bounds.min.x, renderer.bounds.min.y, renderer.bounds.size.x, renderer.bounds.size.y);
            allowedRectAdjusted = allowedRect.MoveBySizeFactor(ScreenPivot);
            spawnRectAdjusted = spawnRect.MoveBySizeFactor(ScreenPivot);

            if (!initiated) {
                resetVelocity();
                setRandomPos(spawnRectAdjusted, true);
                initiated = true;
            }
            
            //If this Renderer outside area
            if (!allowedRectAdjusted.Overlaps(thisRect)) {
                if (thisRect.IsToTheRightOf(allowedRectAdjusted) && rigidBody.velocity.x > 0) {
                    resumeAt(new Vector2(minX(allowedRectAdjusted, false), randomY(spawnRectAdjusted, true)));
                }
                else if (thisRect.IsToTheLeftOf(allowedRectAdjusted) && rigidBody.velocity.x < 0) {                    
                    resumeAt(new Vector2(maxX(allowedRectAdjusted, false), randomY(spawnRectAdjusted, true)));
                }
                else if (thisRect.IsAbove(allowedRectAdjusted) && rigidBody.velocity.y > 0) {
                    resumeAt(new Vector2(randomX(spawnRectAdjusted, true), minY(allowedRectAdjusted, false)));
                }
                else if(rigidBody.velocity.y < 0) {
                    resumeAt(new Vector2(randomX(spawnRectAdjusted, true), maxY(allowedRectAdjusted, false)));
                }
            }                        
        }

        private void resumeAt(Vector2 newPos) {
            StartCoroutine(waitAndResumeAtCoroutine(newPos));
        }

        private IEnumerator waitAndResumeAtCoroutine(Vector2 newPos) {
            transform.position = newPos;
            Vector3 originalVelocity = rigidBody.velocity;
            rigidBody.velocity = Vector3.zero;
            yield return new WaitForSeconds(Random.Range(waitBeforeRestart.min, waitBeforeRestart.max));
            rigidBody.velocity = originalVelocity;
        }

        private void resetVelocity() {
            float newX = Random.Range(velocity.min.x, velocity.max.x);
            float newY = Random.Range(velocity.min.y, velocity.max.y);
            
            rigidBody.velocity = new Vector2(newX, newY);
        }

        private void setRandomPos(Rect rect, bool inside) {                        
            transform.position = new Vector2(randomX(rect, inside), randomY(rect, inside));
        }

        private float randomX(Rect rect, bool inside) {            
            return Random.Range(minX(rect, inside), maxX(rect, inside));
        }
        
        private float randomY(Rect rect, bool inside) {
            return Random.Range(minY(rect, inside), maxY(rect, inside));
        }

        private float minX(Rect rect, bool inside) {
            if (inside) {
                return rect.xMin + relativePivot.x * renderer.bounds.size.x;
            }
            
            return rect.xMin - (1 - relativePivot.x) * renderer.bounds.size.x;
        }
        
        private float maxX(Rect rect, bool inside) {
            if (inside) {
                return rect.xMax - (1 - relativePivot.x) * renderer.bounds.size.x;
            }            
            
            return rect.xMax + relativePivot.x * renderer.bounds.size.x;
        }
        
        private float minY(Rect rect, bool inside) {
            if (inside) {
                return rect.yMin + relativePivot.y * renderer.bounds.size.y;
            }            
            
            return rect.yMin - (1 - relativePivot.y) * renderer.bounds.size.y;
        }
        
        private float maxY(Rect rect, bool inside) {
            if (inside) {
                return rect.yMax - (1 - relativePivot.y) * renderer.bounds.size.y;
            }             
            
            return rect.yMax + relativePivot.y * renderer.bounds.size.y;
        }
    }
      
      
}