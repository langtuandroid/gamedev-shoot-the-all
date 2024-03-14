using System.Collections.Generic;
using UnityEngine;

public class SAMainMenuController : MonoBehaviour
{
    [SerializeField] private List<GameObject> _backToMainMenuButtons;
    [SerializeField] private GameObject _mainMenuPanel;
    [SerializeField] private GameObject _levelsPanel;
    [SerializeField] private GameObject _settingsPanel;

    private void Start()
    {
        SetMenuButtonsActivity(false);
        _levelsPanel.SetActive(false);
        _settingsPanel.SetActive(false);
        _mainMenuPanel.SetActive(true);
    }

    private void SetMenuButtonsActivity(bool state)
    {
        foreach (var button in _backToMainMenuButtons) button.SetActive(state);
    }
    
    public void SettingsButton()
    {
        _mainMenuPanel.SetActive(false);
        SetMenuButtonsActivity(true);
        _settingsPanel.SetActive(true);
        _levelsPanel.SetActive(false);
        AudioManager.instance.Play("Click");
    }

    public void PlayButton()
    {
        SetMenuButtonsActivity(true);
        _levelsPanel.SetActive(true);
        _mainMenuPanel.SetActive(false);
        _settingsPanel.SetActive(false);
        AudioManager.instance.Play("Click");
    }
    
    public void BackToMainMenuButton()
    {
        SetMenuButtonsActivity(false);
        _levelsPanel.SetActive(false);
        _settingsPanel.SetActive(false);
        _mainMenuPanel.SetActive(true);
        AudioManager.instance.Play("Click");
    }
}
