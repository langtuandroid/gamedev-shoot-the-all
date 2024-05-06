using System.Collections.Generic;
using Scripts.Gameplay.Managers;
using Sounds;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Menu
{
    public class SALevelSelectButtonController : MonoBehaviour
    {
        [SerializeField] private Button _thisButton;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private Image _lockImage;
        [SerializeField] private List<GameObject> _starIcons;
        [SerializeField] private GameObject _starsPanel;
        private int _levelIndex;
    
        public void Initialize(int levelIndex)
        {
            _levelIndex = levelIndex;
            var isUnlocked = _levelIndex <= PlayerPrefsManager.GetLevel();
            if (PlayerPrefsManager.HasStars(_levelIndex))
            {
                _starsPanel.SetActive(true);
                var starsOnLevel = PlayerPrefsManager.GetStars(_levelIndex);
                for (int i = starsOnLevel; i < 3; i++) _starIcons[i].SetActive(false);
            }
            else _starsPanel.SetActive(false);
            _lockImage.enabled = !isUnlocked;
            _thisButton.interactable = isUnlocked;
            _levelText.text = _levelIndex.ToString();
            _thisButton.onClick.AddListener(SelectLevel);
        }

        private void SelectLevel()
        {
            SAAudioManager.instance.Play("Click");
            SceneManager.LoadScene(_levelIndex);
        }
    }
}