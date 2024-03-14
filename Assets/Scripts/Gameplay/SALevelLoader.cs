using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gameplay
{
    public class SALevelLoader : MonoBehaviour
    {
        void Start()
        {
            SceneManager.LoadScene(PlayerPrefs.GetInt("level") >= SceneManager.sceneCountInBuildSettings
                ? PlayerPrefs.GetInt("THISLEVEL")
                : PlayerPrefs.GetInt("level", 1));
        }
    }
}
