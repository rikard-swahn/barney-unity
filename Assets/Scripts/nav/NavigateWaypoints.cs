﻿using System;
using System.Collections.Generic;
using System.Linq;
using net.gubbi.goofy.extensions;
using net.gubbi.goofy.util;
using PolyNav;
using UnityEngine;

namespace net.gubbi.goofy.nav {
    public class NavigateWaypoints : MonoBehaviour {

        private IList<Vector2> path;
        private int waypointIndex;
        private bool moving;
        private PolyNavAgent navAgent;
        private Action onArrive;
        private Action<Vector3, Vector2?> onMove;
        private Action<bool> onBlocked;
        private Action<int> onNextWaypoint;

        private void Awake() {
            navAgent = GetComponentInChildren<PolyNavAgent>();
        }

        private void Update() {
            if (moving && onMove != null) {
                onMove (navAgent.gameObject.transform.position, navAgent.movingDirection);
            }
        }

        public void start(GameObject waypointGroup, int? wpIndex, Action onArrive, Action<Vector3, Vector2?> onMove, Action<int> onNextWaypoint, Action<bool> onBlocked = null) {
            start (getPath (waypointGroup), wpIndex, onArrive, onMove, onNextWaypoint, onBlocked);
        }

        public void start(IList<Vector2> path, int? wpIndex, Action onArrive, Action<Vector3, Vector2?> onMove = null, Action<int> onNextWaypoint = null, Action<bool> onBlocked = null) {
            waypointIndex = wpIndex != null ? (int)wpIndex : 0;

            this.path = path;
            this.onArrive = onArrive;
            this.onMove = onMove;
            this.onBlocked = onBlocked;
            this.onNextWaypoint = onNextWaypoint;

            goToNextWaypoint ();
        }

        public void stop () {
            moving = false;
            navAgent.Stop ();
        }

        public void moveToEnd(GameObject waypointGroup, Action<Vector3, Vector2?> onMove, Action<int> onNextWaypoint,
            Vector2? lastPos, bool endingCurrentBehavior) {

            moveToEnd(getPath (waypointGroup), onMove, onNextWaypoint, lastPos, endingCurrentBehavior);
        }

        public void moveToEnd(IList<Vector2> path, Action<Vector3, Vector2?> onMove, Action<int> onNextWaypoint, Vector2? lastPos, bool endingCurrentBehavior) {
            this.path = path;
            onArrive = null;
            this.onMove = onMove;
            this.onNextWaypoint = onNextWaypoint;
            onBlocked = null;

            if (!endingCurrentBehavior) {
                waypointIndex = 0;
            }
			 
            if (onMove != null) {
                onMove (
                    new Vector3 (path.Last ().x, path.Last ().y, 0), 
                    getEndDirection (lastPos)
                );
            }

            waypointReached(path.Count - 1);
        }

        public List<Vector2> getPath (GameObject waypointGroup) {
            IList<GameObject> children = waypointGroup.findChildrenWithTag (Tags.WAYPOINT);

            return children
                .Select (c => new Vector2(c.transform.position.x, c.transform.position.y))
                .ToList ();
        }

        private Vector2? getEndDirection (Vector2? lastPos) {
            if (path.Count > 1) {
                Vector2 vLast = path.Last ();
                Vector2 vNextLast = path.ElementAt (path.Count - 2);					
                return new Vector2 (vLast.x - vNextLast.x, vLast.y - vNextLast.y);
            } else if (path.Count == 1) {
                Vector2 vLast = path.Last ();

                if (waypointIndex == 0 && lastPos != null) {					
                    return new Vector2 (vLast.x - ((Vector2)lastPos).x, vLast.y - ((Vector2)lastPos).y);
                }
            }

            return null;
        }			

        private void goToNextWaypoint () {
            moving = true;

            if (waypointIndex < path.Count) {
                Action<bool> wpReached = delegate {
                    waypointReached(waypointIndex);
                };
                navAgent.SetDestination (path[waypointIndex], wpReached, null, false, onBlocked);
            } else {
                moving = false;
                if (onArrive != null) {
                    onArrive ();
                }
            }
        }

        private void waypointReached(int waypointIndex) {                                     
            if (onNextWaypoint != null) {
                onNextWaypoint (this.waypointIndex);
            }
            
            this.waypointIndex = waypointIndex + 1;

            goToNextWaypoint();			
        }

    }
}