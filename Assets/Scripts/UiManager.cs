using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{
    public GameObject levelCompletedPanel;
    public GameObject levelFailPanel;
    public GameObject gamePanel;
    public TextMeshProUGUI LevelNumber;
    public static UiManager Instance;
    public bool desiontaken;
    
    
    
    private void Awake()
    {
        if(!Instance)
        {
            Instance = this;
        }
    }
    void Start()
    {
        levelCompletedPanel.SetActive(false);
        levelFailPanel.SetActive(false);
        gamePanel.SetActive(true);
        DisplayLevelNumber();
    }
    
    public void DisplayLevelNumber()
    {
        //LevelNumber = GetComponent<TextMeshProUGUI>();
        LevelNumber.text = "Level " + PlayerPrefs.GetInt("level", 1);
    }
    
    public void LevelCompleted()
    {
        if (!desiontaken)
        {
            desiontaken = true;
            if (Particaleffect.instance)
            {
                Particaleffect.instance.playpop();
            }
            
            StartCoroutine(win());
        }
    }
    public void LevelFail()
    {
        if (!desiontaken)
        {
            desiontaken = true;
            StartCoroutine(loss());
        }
    }

    IEnumerator loss()
    {
        yield return new WaitForSeconds(1f);
        gamePanel.SetActive(false);
        levelFailPanel.SetActive(true);
        levelCompletedPanel.SetActive(false);
        if (AudioManager.instance)
        {
            AudioManager.instance.Play("loss");
        }
    }

    IEnumerator win()
    {
        yield return new WaitForSeconds(1f);
        if (!desiontaken)
        {
            gamePanel.SetActive(false);
            levelCompletedPanel.SetActive(true);
            if (AudioManager.instance) AudioManager.instance.Play("WIn");
        }
    }

    public void NextLevel()
    {
        if (PlayerPrefs.GetInt("level") >= (SceneManager.sceneCountInBuildSettings) - 1)
        {
            PlayerPrefs.SetInt("level", PlayerPrefs.GetInt("level", 1) + 1);
            int i = Random.Range(1, (SceneManager.sceneCountInBuildSettings));
            PlayerPrefs.SetInt("THISLEVEL", i);
            SceneManager.LoadScene(i);
        }
        else
        {
            PlayerPrefs.SetInt("level", SceneManager.GetActiveScene().buildIndex + 1);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
