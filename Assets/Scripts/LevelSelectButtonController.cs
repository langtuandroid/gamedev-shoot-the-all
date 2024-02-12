using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectButtonController : MonoBehaviour
{
    [SerializeField] private Button _thisButton;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private Image _lockImage;
    private int _levelIndex;
    
    public void Initialize(int levelIndex)
    {
        _levelIndex = levelIndex;
        _lockImage.enabled = _levelIndex > PlayerPrefs.GetInt("level", 1);
        _thisButton.interactable = _levelIndex <= PlayerPrefs.GetInt("level", 1);
        _levelText.text = _levelIndex.ToString();
        _thisButton.onClick.AddListener(SelectLevel);
    }

    private void SelectLevel()
    {
        SceneManager.LoadScene(_levelIndex);
    }
}