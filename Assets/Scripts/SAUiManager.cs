using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SAUiManager : MonoBehaviour
{
    public static SAUiManager Instance;
    [SerializeField] private GameObject _levelEndedPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject menuButton;
    [SerializeField] private GameObject _restartButton;
    [SerializeField] private GameObject _nextLevelButton;
    [SerializeField] private GameObject _levelWinText;
    [SerializeField] private GameObject _levelLoseText;
    [SerializeField] private TextMeshProUGUI _levelNumberText;
    [SerializeField] private TextMeshProUGUI _levelEndNumberText;
    [SerializeField] private List<GameObject> _starIcons;
    
    private bool _desiontaken;
    private Coroutine _winCoroutine;
    
    private void Awake()
    {
        if(!Instance) Instance = this;
    }
    
    void Start()
    {
        menuButton.SetActive(true);
        _levelEndedPanel.SetActive(false);
        gamePanel.SetActive(true);
        SetLevel();
    }

    private void SetLevel()
    {
        _levelNumberText.text = "Level " + SceneManager.GetActiveScene().buildIndex;
        _levelEndNumberText.text = SceneManager.GetActiveScene().buildIndex.ToString();
    }

    public void OnLevelCompleted()
    {
        StarsPanelController.Instance.DisableStars(_starIcons);
        _levelWinText.SetActive(true);
        _levelLoseText.SetActive(false);
        _restartButton.SetActive(false);
        _nextLevelButton.SetActive(true);
        menuButton.SetActive(false);
        int activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
        StarsPanelController.Instance.SaveStars(activeSceneIndex);
        if (PlayerPrefs.GetInt("level") < activeSceneIndex + 1 && activeSceneIndex < SceneManager.sceneCountInBuildSettings - 1) PlayerPrefs.SetInt("level", activeSceneIndex + 1);
        if (SceneManager.GetActiveScene().buildIndex >= SceneManager.sceneCountInBuildSettings - 1) _nextLevelButton.SetActive(false);
        if (!_desiontaken)
        {
            _desiontaken = true;
            if (SAParticleEffect.instance)
            {
                SAParticleEffect.instance.PlayParticle();
            }
            
            _winCoroutine = StartCoroutine(WinCoroutine());
        }
    }


    public void LevelFail()
    {
        foreach (var star in _starIcons) star.SetActive(false);
        _levelWinText.SetActive(false);
        _levelLoseText.SetActive(true);
        _restartButton.SetActive(true);
        _nextLevelButton.SetActive(false);
        menuButton.SetActive(false);
        _desiontaken = true;
        StartCoroutine(LoseCoroutine());

    }

    private IEnumerator LoseCoroutine()
    {
        if (_winCoroutine != null) StopCoroutine(_winCoroutine);
        yield return new WaitForSeconds(1f);
        gamePanel.SetActive(false);
        _levelEndedPanel.SetActive(true);
        if (AudioManager.instance)
        {
            AudioManager.instance.Play("loss");
        }
    }

    private IEnumerator WinCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        gamePanel.SetActive(false);
        _levelEndedPanel.SetActive(true);
        if (AudioManager.instance) AudioManager.instance.Play("WIn");
    }

    public void SetNextLevel()
    {
        int activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (activeSceneIndex >= SceneManager.sceneCountInBuildSettings - 1) SceneManager.LoadScene(0);
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
    
    public void Retry() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}
