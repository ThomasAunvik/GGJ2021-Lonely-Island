using LonelyIsland.Characters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

namespace LonelyIsland.System
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;
        public static GameManager Instance { get { return _instance; } }

        public int firstScene = 1;
        public bool firstSave = true;
        public bool resetSave = false;
        private Save save = null;

        public bool GetIsFirstSave() { return firstSave || resetSave; }

        public Save Save { get { return save; } }
        public Stats Stats { get { return save.Stats; } }

        public bool Teleporting = false;

        private void Awake()
        {
            if (_instance) { Destroy(gameObject); return; }
            else _instance = this;

            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += SceneLoaded;

            firstSave = true;
            save = LoadGame();

            LoadFirstScene();
        }

        private void LoadFirstScene()
        {
            if (SceneManager.sceneCount == 1)
            {
                int startupScene = save.WorldIndex == -1 ? firstScene : save.WorldIndex;
                if (Startup.StarupSceneIndex != 0) startupScene = Startup.StarupSceneIndex;
                SceneManager.LoadScene(startupScene, LoadSceneMode.Single);
            }
        }

        private void SceneLoaded(Scene scene, LoadSceneMode mode)
        {

        }

        public bool UseMoney(int money)
        {
            if (save.Coins - money < 0) return false;

            save.Coins -= money;
            return true;
        }

        public void ResetSave()
        {
            resetSave = true;

            save = new Save();
            SaveGame(true);

            int startupScene = Startup.StarupSceneIndex == 0 ? firstScene : Startup.StarupSceneIndex;
            SceneManager.LoadScene(startupScene, LoadSceneMode.Single);
        }

        public void NewGame()
        {
            save = new Save();
            SaveGame(false);

            int startupScene = Startup.StarupSceneIndex == 0 ? firstScene : Startup.StarupSceneIndex;
            SceneManager.LoadScene(startupScene, LoadSceneMode.Single);
        }

        public void SaveGame(bool isReset = false)
        {
            try
            {
                if (!isReset) resetSave = false;

                save.WorldIndex = SceneManager.GetActiveScene().buildIndex;

                string directory = Path.Combine(Application.persistentDataPath, "Saves");
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                    if(!Directory.Exists(directory))
                        throw new Exception("Save Directory was failed to be created.");
                }

                string fileName = "save.json";
                string filePath = Path.Combine(directory, fileName);

                string json = JsonConvert.SerializeObject(save, Formatting.Indented);
                File.WriteAllText(filePath, json);
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to save!\n" + e, this);
            }
        }

        private Save LoadGame()
        {
            try
            {
                string directory = Path.Combine(Application.persistentDataPath, "Saves");
                if (!Directory.Exists(directory)) throw new Exception("Save Directory does not exist");

                string fileName = "save.json";
                string filePath = Path.Combine(directory, fileName);

                string json = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<Save>(json);
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to load save file!\n" + e, this);
                firstSave = true;
            }
            return new Save();
        }

        public void TeleportToWorld(int worldIndex)
        {
            Teleporting = true;

            SceneManager.LoadScene(worldIndex);
        }
    }
}