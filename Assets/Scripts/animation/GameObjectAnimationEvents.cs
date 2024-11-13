using System;
using System.Collections.Generic;
using UnityEngine;

namespace net.gubbi.goofy.animation {
    public class GameObjectAnimationEvents : MonoBehaviour {
        
        private Dictionary<string, GameObject> gameObjects = new Dictionary<string, GameObject>();
        
        public void setActive(string name) {
            findGameObject(name).SetActive(true);
        }
        
        public void setInactive(string name) {
            findGameObject(name).SetActive(false);
        }

        private GameObject findGameObject(string name) {
            if (gameObjects.ContainsKey(name)) {
                return gameObjects[name];
            }

            GameObject go = GameObject.Find(name);

            if (go != null) {
                gameObjects[name] = go;
                return go;
            }
            
            throw new ArgumentException("Gameobject '" + name + "' not found!");
        }
    }
}