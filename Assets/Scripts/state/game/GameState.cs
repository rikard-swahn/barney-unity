using System;
using System.IO;
using net.gubbi.goofy.scene.load;
using net.gubbi.goofy.scene.load.transition;
using net.gubbi.goofy.util;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = System.Object;

namespace net.gubbi.goofy.state {

    public class GameState {
        
        public readonly static float VERSION = 1.0f;

        static GameState () {
            SceneManager.sceneUnloaded += onSceneUnloaded;			
        }

        private static StateData stateData = new StateData();

        private static StateData loadingStateData;

        private static volatile GameState instance;
        private static object syncRoot = new Object();
        private GameState() {}
        public static GameState Instance {
            get {
                if (instance == null) {
                    lock (syncRoot) {
                        if (instance == null) {
                            instance = new GameState();
                        }
                    }
                }
                return instance;
            }
        }

        /// <summary>
        /// Write the game state to file 
        /// </summary>
        /// <param name="index">The game slot to save to</param>
        public void save(int index) {
            if (!StateData.Initialized) {
                Debug.Log("Not saving slot " + index + ". Not initialized.");
                return;
            }
            if (StateData.Scene == null) {
                Debug.LogWarning("Not saving. No scene set.");
                return;
            }            
            
            StateDataDto dto = new StateDataDto (stateData);
            string json = JsonUtil.Instance.Serialize(dto);
            using (StreamWriter streamWriter = File.CreateText (getFilename(index))) {
                streamWriter.Write (json);
            }
        }

        /// <summary>
        /// Read the given save location and set the game state.
        /// </summary>
        /// <param name="index">The game slot to load from</param> 
        public void load(int index, SceneTransition outTransition = null, SceneTransition inTransition = null) {                        
            string fileName = getFilename (index);

            if (!File.Exists(fileName)) {
                throw new ArgumentException("Save game " + fileName + " not found!");                
            }
            
            using (StreamReader streamReader = File.OpenText (fileName)) {
                string json = streamReader.ReadToEnd ();
                var dto = JsonUtil.Instance.Deserialize<StateDataDto>(json);
                loadingStateData = new StateData(dto);
            }            
            
            SceneLoader.Instance.loadScene (loadingStateData.Scene, outTransition, inTransition);
        }

        public StateData StateData {
            get {
                return stateData;
            }
        }
        
        public StateData LoadingStateData {
            get {
                return loadingStateData;
            }
        }

        private string getFilename(int index) {
            string filename = "game" + index + ".json";
            return Path.Combine(Application.persistentDataPath, filename);                        
        }

        private static void onSceneUnloaded(Scene scene) {
            if (loadingStateData != null) {
                stateData = loadingStateData;
                loadingStateData = null;
            }
        }

        public void reset() {
            stateData = new StateData();
        }

        public void delete(int index) {
            File.Delete(getFilename(index));
        }

        public void addGameTime(float deltaTime) {
            StateData.PartGameTimeSeconds += deltaTime;
        }
        
        public void addGameTimeForStartedGame(float deltaTime) {
            StateData.PartStartedGameTimeSeconds += deltaTime;
        }
    }

}