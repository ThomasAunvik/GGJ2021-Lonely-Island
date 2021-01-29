using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LonelyIsland.System
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager _instance;

        public int firstScene = 1;
        private Save save = null;

        private void Awake()
        {
            if (_instance) { Destroy(gameObject); return; }
            else _instance = this;

            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += SceneLoaded;

            save = LoadGame();
            if (SceneManager.sceneCount == 1)
            {
                int startupScene = Startup.StarupSceneIndex == 0 ? firstScene : Startup.StarupSceneIndex;
                SceneManager.LoadScene(startupScene, LoadSceneMode.Single);
            }
        }

        private void SceneLoaded(Scene scene, LoadSceneMode mode)
        {

        }

        private void SaveGame()
        {
            try
            {
                string directory = Path.Combine(Application.persistentDataPath, "Saves");
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                    if(!Directory.Exists(directory))
                        throw new Exception("Save Directory was failed to be created.");
                }

                string fileName = "save.json";
                string filePath = Path.Combine(directory, fileName);

                string json = JsonUtility.ToJson(save, true);
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
                return JsonUtility.FromJson<Save>(json);
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to load save file!\n" + e, this);
            }
            return new Save();
        }
    }
}