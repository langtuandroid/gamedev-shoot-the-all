using System.Collections.Generic;
using UnityEngine;

public class StarsPanelController : MonoBehaviour
{
    public static StarsPanelController Instance;

    public const string k_StarsOnLevelKey = "STARS_LEVEL";

    [SerializeField] private List<GameObject> _starIcons;

    private int _currentStarAmount = 4;
    
    void Awake() => Instance = this;

    public void StarLost()
    {
        if (_currentStarAmount <= 1) return;
        _currentStarAmount--;
        if (_currentStarAmount >= 3) return;
        _starIcons[_currentStarAmount].SetActive(false);
    }

    public void SaveStars(int level)
    {
        if (!PlayerPrefs.HasKey(k_StarsOnLevelKey + level) || PlayerPrefs.GetInt(k_StarsOnLevelKey + level) < _currentStarAmount)
            PlayerPrefs.SetInt(k_StarsOnLevelKey + level, _currentStarAmount);
    }

    public void DisableStars(List<GameObject> starIcons)
    {
        for (int i = 0; i < starIcons.Count; i++)
        {
            starIcons[i].SetActive(_starIcons[i].activeSelf);
        }
    }
}
