﻿using System;
using System.Collections.Generic;
using net.gubbi.goofy.say;
using net.gubbi.goofy.util;
using PolyNav;
using UnityEngine;

namespace net.gubbi.goofy.character {
    public class CharacterFacade : MonoBehaviour {

        public List<BodyState> staticDirectionStates;

        private Say say;
        private CharacterMove characterMove;
        private PolyNavAgent navAgent;

        private void Awake () {
            say = GameObject.Find(GameObjects.SPEECH_BUBBLE_CONTAINER).GetComponent<Say>();
            characterMove = GetComponent<CharacterMove>();
            navAgent = GetComponentInChildren<PolyNavAgent>();
        }

        public void sayTextTowardsCamera (string text) {
            sayTextTowardsCamera (text, null);
        }

        public void sayTextTowardsCamera (string text, Action sayCompleteCallback) {
            turnTowardsCamera ();
            sayText (text, sayCompleteCallback);
        }			

        public void sayText (string text) {
            sayText (text, null);
        }

        public void sayText (string text, Action sayCompleteCallback) {
            say.say (gameObject, text, sayCompleteCallback);
        }
        
        public void sayTextRaw (string text, Action sayCompleteCallback) {
            say.sayRaw(gameObject, text, sayCompleteCallback);
        }

        public void sayBackground (string text, Action sayCompleteCallback, float delay, bool doCallbackOnOverride, Action sayAbortedCallback) {
            say.sayBackground (gameObject, text, sayCompleteCallback, delay, doCallbackOnOverride, sayAbortedCallback);
        }

        public void turnTowardsCamera() {
            setDirectionImmediate(Vector2.down);
        }

        public void turnTowards(Vector3 pos) {
            if (navAgent != null) {
                Vector2 dir = new Vector2(pos.x - navAgent.position.x, pos.y - navAgent.position.y);
                setDirectionImmediate(dir);
            }
        }
        
        public void turnTowardsState(Vector3 pos) {
            if (navAgent != null) {
                Vector2 dir = new Vector2(pos.x - navAgent.position.x, pos.y - navAgent.position.y);
                characterMove.setDirectionState(dir);
            }
        }

        public void setDirectionImmediate(Vector2 dir) {
            if (isStaticDirection ()) {
                return;
            }

            characterMove.setDirectionImmediate(dir);
        }

        public Vector2 getNormalizedDirection() {
            float dx = characterMove.LastDirection.x;
            float dy = characterMove.LastDirection.y;

            if (Math.Abs(dx) >= Math.Abs(dy)) {
                dx = Math.Sign(dx);
                dy = 0;
            }
            else {
                dx = 0;
                dy = Math.Sign(dy);
            }
            
            return new Vector2(dx, dy);                        
        }

        public void navStop () {
            navAgent.Stop ();
        }

        public void stopAndTurnTowardsCamera () {
            navAgent.Stop ();
            turnTowardsCamera ();
        }
			
        public void stopAndTurnTowards(Vector3 pos) {
            navAgent.Stop ();
            turnTowards (pos);
        }

        public Vector3 getPosition() {
            return characterMove.getPosition();
        }

        private bool isStaticDirection() {
            BodyState? state = characterMove.getBodyState ();
            return state != null && staticDirectionStates.Contains((BodyState)state);
        }

    }
}