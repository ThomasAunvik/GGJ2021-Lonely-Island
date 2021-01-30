using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LonelyIsland.System {
    public class Startup : MonoBehaviour
    {
        public static int StarupSceneIndex { get; set; } = 0;
        public int startupIndex = 0;

        void Start()
        {
            if (GameManager._instance) return;
            StarupSceneIndex = startupIndex;

            SceneManager.LoadScene(0);
        }
    }
}
