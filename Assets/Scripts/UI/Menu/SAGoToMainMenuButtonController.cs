using Sounds;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Menu
{
    public class SAGoToMainMenuButtonController : MonoBehaviour
    {
        [SerializeField] private Button _thisButton;

        private void Start() => _thisButton.onClick.AddListener(() =>
        {
            SAAudioManager.instance.Play("Click");
            SceneManager.LoadScene(0);
        });
    }
}