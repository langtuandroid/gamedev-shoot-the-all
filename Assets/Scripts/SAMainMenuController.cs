using UnityEngine;

public class SAMainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject _backToMainMenuButton;
    [SerializeField] private GameObject _mainMenuPanel;
    [SerializeField] private GameObject _levelsPanel;

    private void Start()
    {
        
        _backToMainMenuButton.SetActive(false);
        _levelsPanel.SetActive(false);
        _mainMenuPanel.SetActive(true);
    }

    public void SettingsButton()
    {
        
    }

    public void PlayButton()
    {
        _backToMainMenuButton.SetActive(true);
        _levelsPanel.SetActive(true);
        _mainMenuPanel.SetActive(false);
    }

    public void BackToMainMenuButton()
    {
        _backToMainMenuButton.SetActive(false);
        _levelsPanel.SetActive(false);
        _mainMenuPanel.SetActive(true);
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
