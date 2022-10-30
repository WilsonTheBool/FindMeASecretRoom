using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class SceneLoader : MonoBehaviour
    {
        public string sceneName;

        public void LoadScene()
        {
            SceneManager.LoadScene(sceneName);
        }

        public void LoadScene_Async()
        {
            SceneManager.LoadSceneAsync(sceneName);
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}