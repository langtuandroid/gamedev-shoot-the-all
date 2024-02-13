using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SAUiManager : MonoBehaviour
{
    public static SAUiManager Instance;
    [SerializeField] private GameObject levelCompletedPanel;
    [SerializeField] private GameObject levelFailPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject menuButton;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private TextMeshProUGUI LevelNumber;
    
    private bool _desiontaken;
    private Coroutine _winCoroutine;
    
    private void Awake()
    {
        if(!Instance) Instance = this;
    }
    
    void Start()
    {
        menuButton.SetActive(true);
        levelCompletedPanel.SetActive(false);
        levelFailPanel.SetActive(false);
        gamePanel.SetActive(true);
        SetLevel();
    }

    private void SetLevel() => LevelNumber.text = "Level " + SceneManager.GetActiveScene().buildIndex;

    public void OnLevelCompleted()
    {
        menuButton.SetActive(false);
        int activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (PlayerPrefs.GetInt("level") < activeSceneIndex + 1 && activeSceneIndex < SceneManager.sceneCountInBuildSettings - 1) PlayerPrefs.SetInt("level", activeSceneIndex + 1);
        if (SceneManager.GetActiveScene().buildIndex >= SceneManager.sceneCountInBuildSettings - 1) nextButton.SetActive(false);
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
        menuButton.SetActive(false);
        _desiontaken = true;
        StartCoroutine(LoseCoroutine());

    }

    private IEnumerator LoseCoroutine()
    {
        if (_winCoroutine != null) StopCoroutine(_winCoroutine);
        levelCompletedPanel.SetActive(false);
        yield return new WaitForSeconds(1f);
        gamePanel.SetActive(false);
        levelFailPanel.SetActive(true);
        if (AudioManager.instance)
        {
            AudioManager.instance.Play("loss");
        }
    }

    private IEnumerator WinCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        gamePanel.SetActive(false);
        levelCompletedPanel.SetActive(true);
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
