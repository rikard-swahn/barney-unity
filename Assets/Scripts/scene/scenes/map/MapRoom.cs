using System;
using System.Collections.Generic;
using System.Linq;
using net.gubbi.goofy.nav;
using net.gubbi.goofy.player;
using net.gubbi.goofy.state;
using net.gubbi.goofy.util;
using UnityEngine;

namespace net.gubbi.goofy.scene.scenes.map {
    public class MapRoom : Room {

        public GameObject homeToGrandmaWps;
        public GameObject homeToMallWps;
        public GameObject homeToMallWps2;
        public GameObject homeToDonaldWps;
        public GameObject homeToGyroWps;
        public GameObject grandmaToMallWps;
        public GameObject grandmaToDonaldWps;
        public GameObject grandmaToDonaldWps2;
        public GameObject grandmaToGyrodWps;
        public GameObject grandmaToGyrodWps2;
        public GameObject mallToDonaldWps;
        public GameObject mallToGyroWps;
        public GameObject donaldToGyroWps;
        public GameObject homeToMickeyWps;
        public GameObject grandmaToMickeyWps;
        public GameObject mallToMickeyWps;
        public GameObject donaldToMickeyWps;
        public GameObject gyroToMickeyWps;

        private Dictionary<int, Dictionary<int, List<List<Vector2>>>> walkPaths;
        private List<Vector2> locations = new List<Vector2> ();
        private int currentPos;
        private int destPos = -1;
        private NavigateWaypoints navigateWaypoints;
        private PlayerMove playerMove;
        private List<Vector2> activePath;

        protected override void Awake() {
            base.Awake();
            GameObject playerGo = GameObject.Find(GameObjects.PLAYER);
            navigateWaypoints = playerGo.GetComponent<NavigateWaypoints>();
            currentPos = GameState.Instance.StateData.PlayerState.MapPosition;
            playerMove = playerGo.GetComponent<PlayerMove>();
                        
            setupPaths ();
            setupLocations ();

            if (GameState.Instance.StateData.PlayerState.LastWaypoint == null) {
                playerMove.setPosition(locations[currentPos]);
            }
        }

        public void goTo(int destPos, Action onArrived) {
            Action onArriveWrapper = delegate {
                currentPos = destPos;
                GameState.Instance.StateData.PlayerState.LastWaypoint = null;
                GameState.Instance.StateData.PlayerState.NextWaypoint = null;
                activePath = null;
                destPos = -1;
                onArrived ();			
            };

            if (destPos == currentPos && GameState.Instance.StateData.PlayerState.LastWaypoint == null) {
                onArriveWrapper ();
                return;
            }

            if (destPos == this.destPos) {
                return;
            }
            
            activePath = getPath (destPos, GameState.Instance.StateData.PlayerState.LastWaypoint);
            this.destPos = destPos;

            navigateWaypoints.start (activePath, 0, onArriveWrapper, null, onNextWaypoint);
        }

        private void onNextWaypoint(int wpIndex) {            
            GameState.Instance.StateData.PlayerState.LastWaypoint = activePath.ElementAt(wpIndex);

            int nextWpIndex = wpIndex + 1;
            if (nextWpIndex < activePath.Count) {
                GameState.Instance.StateData.PlayerState.NextWaypoint = activePath.ElementAt(nextWpIndex);
            }
        }

        private List<Vector2> getPath (int destPos, Vector2? lastWpNullable) {                        
            if (lastWpNullable != null) {                
                return getShortestPartialPath((Vector2)lastWpNullable, destPos);
            }

            return getCompletePath(destPos);
        }

        private List<Vector2> getShortestPartialPath(Vector2 lastWp, int destPos) {            
            List<Vector2> pathForLastWp = getPartialPath(lastWp, destPos);

            Vector2? nextWp = GameState.Instance.StateData.PlayerState.NextWaypoint;
            if (nextWp != null) {
                List<Vector2> pathForNextWp = getPartialPath((Vector2)nextWp, destPos);
                if (pathLength(pathForNextWp) < pathLength(pathForLastWp)) {
                    return pathForNextWp;
                }
            }

            return pathForLastWp;
        }

        private List<Vector2> getPartialPath(Vector2 wp, int destPos) {
            List<Vector2> selectedPath = new List<Vector2>();
            float? minLength = null;
            float length;
            
            foreach (var path in walkPaths) {
                if (path.Value.ContainsKey(destPos)) {
                    
                    List<List<Vector2>> pathsToDest = path.Value[destPos];

                    foreach (var pathToDest in pathsToDest) {
                        int? nullableIndex = Vector2Util.looseIndexOf(pathToDest, wp);
                        if (nullableIndex != null) {
                            int i = (int)nullableIndex;
                            List<Vector2> curPath = pathToDest.GetRange(i, pathToDest.Count - i);
                            length = pathLength(pathToDest.GetRange(i, pathToDest.Count - i));
                            
                            if (minLength == null || length < minLength) {
                                minLength = length;
                                selectedPath = curPath;
                            }                            
                        }                        
                    }
                }
            }            
            
            foreach (var path in walkPaths[destPos]) {
                List<List<Vector2>> pathsFromDest = path.Value;

                foreach (var pathFromDest in pathsFromDest) {
                    List<Vector2> revPath = new List<Vector2>(pathFromDest);
                    revPath.Reverse();                       
                    
                    int? nullableIndex = Vector2Util.looseIndexOf(revPath, wp);
                    
                    if (nullableIndex != null) {                    
                        int i = (int)nullableIndex;
                        List<Vector2> curPath = revPath.GetRange(i, revPath.Count - i);
                        length = pathLength(curPath);
                        
                        if (minLength == null || length < minLength) {
                            minLength = length;
                            selectedPath = curPath;
                        }                                                 
                    }                    
                }
            }
            
            return selectedPath;
        }

        private float pathLength(List<Vector2> path) {
            float length = 0;
            
            for (int i = 0; i < path.Count - 1; i++) {
                length += (path[i + 1] - path[i]).magnitude;
            }

            return length;
        }

        private List<Vector2> getCompletePath(int destPos) {
            if (walkPaths.ContainsKey(currentPos) && walkPaths[currentPos].ContainsKey(destPos)) {
                return walkPaths[currentPos][destPos].First();
            }

            List<Vector2> path = new List<Vector2>(walkPaths[destPos][currentPos].First());
            path.Reverse();
            return path;
        }

        private void setupPaths () {
            walkPaths = new Dictionary<int, Dictionary<int, List<List<Vector2>>>> ();

            for (int i = 0; i <= 6; i++) {
                walkPaths[i] = new Dictionary<int, List<List<Vector2>>>();
            }

            walkPaths[0][1] = new List<List<Vector2>>(new[]{navigateWaypoints.getPath(homeToGrandmaWps)});
            walkPaths[0][2] = new List<List<Vector2>>(new[]{navigateWaypoints.getPath(homeToMallWps), navigateWaypoints.getPath(homeToMallWps2)});
            walkPaths[0][3] = new List<List<Vector2>>(new[]{navigateWaypoints.getPath(homeToDonaldWps)});
            walkPaths[0][4] = new List<List<Vector2>>(new[]{navigateWaypoints.getPath(homeToGyroWps)});
            walkPaths[0][5] = new List<List<Vector2>>(new[]{navigateWaypoints.getPath(homeToMickeyWps)});

            walkPaths[1][2] = new List<List<Vector2>>(new[]{navigateWaypoints.getPath(grandmaToMallWps)});
            walkPaths[1][3] = new List<List<Vector2>>(new[]{navigateWaypoints.getPath(grandmaToDonaldWps), navigateWaypoints.getPath(grandmaToDonaldWps2)});
            walkPaths[1][4] = new List<List<Vector2>>(new[]{navigateWaypoints.getPath(grandmaToGyrodWps), navigateWaypoints.getPath(grandmaToGyrodWps2)});
            walkPaths[1][5] = new List<List<Vector2>>(new[]{navigateWaypoints.getPath(grandmaToMickeyWps)});

            walkPaths[2][3] = new List<List<Vector2>>(new[]{navigateWaypoints.getPath(mallToDonaldWps)});
            walkPaths[2][4] = new List<List<Vector2>>(new[]{navigateWaypoints.getPath(mallToGyroWps)});
            walkPaths[2][5] = new List<List<Vector2>>(new[]{navigateWaypoints.getPath(mallToMickeyWps)});

            walkPaths[3][4] = new List<List<Vector2>>(new[]{navigateWaypoints.getPath(donaldToGyroWps)});
            walkPaths[3][5] = new List<List<Vector2>>(new[]{navigateWaypoints.getPath(donaldToMickeyWps)});

            walkPaths[4][5] = new List<List<Vector2>>(new[]{navigateWaypoints.getPath(gyroToMickeyWps)});
        }

        private void setupLocations () {
            locations.Add (walkPaths [0][1].First ().First());
            locations.Add (walkPaths [0][1].First().Last ());
            locations.Add (walkPaths [0][2].First().Last ());
            locations.Add (walkPaths [0][3].First().Last ());
            locations.Add (walkPaths [0][4].First().Last ());
            locations.Add (walkPaths [0][5].First().Last ());
        }

    }
}