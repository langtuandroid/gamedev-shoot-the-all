using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuButtonController : MonoBehaviour
{
    [SerializeField] private Button _thisButton;

    private void Start() => _thisButton.onClick.AddListener(() => SceneManager.LoadScene(0));
}