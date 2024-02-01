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
        displayLevelNumber();
    }
    void Update()
    {

    }
    public void displayLevelNumber()
    {
        //LevelNumber = GetComponent<TextMeshProUGUI>();
        LevelNumber.text = "Level " + PlayerPrefs.GetInt("level", 1).ToString();
    }
    public void levelCompleted()
    {
        if (!desiontaken)
        {
            if (Particaleffect.instance)
            {
                Particaleffect.instance.playpop();
            }
            
            StartCoroutine(win());
        }
    }
    public void LevelFail()
    {
        // if (AdManager.instance)
        // {
        //     AdManager.instance.showInterstitial();
        // }
        if (!desiontaken)
        {
            StartCoroutine(loss());
        }
    }

    IEnumerator loss()
    {
        yield return new WaitForSeconds(1f);
        gamePanel.SetActive(false);
        levelFailPanel.SetActive(true);
        desiontaken = true;
        if (AudioManager.instance)
        {
            AudioManager.instance.Play("loss");
        }
    }

    IEnumerator win()
    {
        yield return new WaitForSeconds(1f);
        // if (AdManager.instance)
        // {
        //     AdManager.instance.showInterstitial();
        // }
        // yield return new WaitForSeconds(1.25f);
        gamePanel.SetActive(false);
        levelCompletedPanel.SetActive(true);
        desiontaken = true;
        if (AudioManager.instance)
        {
            AudioManager.instance.Play("WIn");
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
