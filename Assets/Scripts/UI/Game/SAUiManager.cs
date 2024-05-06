using System.Collections;
using System.Collections.Generic;
using Integration;
using Managers;
using Scripts.Gameplay.Managers;
using Sounds;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace UI.Game
{
    public class SAUiManager : MonoBehaviour
    {
        public static SAUiManager Instance;
    
        [SerializeField] private GameObject _levelEndedPanel;
        [SerializeField] private GameObject _menuButton;
        [SerializeField] private GameObject _restartButton;
        [SerializeField] private GameObject _nextLevelButton;
        [SerializeField] private GameObject _levelWinText;
        [SerializeField] private GameObject _levelLoseText;
        [SerializeField] private TextMeshProUGUI _levelNumberText;
        [SerializeField] private TextMeshProUGUI _levelEndNumberText;
        [SerializeField] private List<GameObject> _starIcons;
    
        private bool _isLevelCompleted;
        private Coroutine _winCoroutine;
    
        private void Awake() { if (!Instance) Instance = this; }

        void Start()
        {
            _menuButton.SetActive(true);
            _levelEndedPanel.SetActive(false);
            SetLevelTexts();
        }

        private void SetLevelTexts()
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
            _menuButton.SetActive(false);
            int activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
            StarsPanelController.Instance.SaveStars(activeSceneIndex);
            if (PlayerPrefsManager.GetLevel() < activeSceneIndex + 1 && activeSceneIndex < SceneManager.sceneCountInBuildSettings - 1) PlayerPrefsManager.SetLevel(activeSceneIndex + 1);
            if (SceneManager.GetActiveScene().buildIndex >= SceneManager.sceneCountInBuildSettings - 1) _nextLevelButton.SetActive(false);
            if (!_isLevelCompleted)
            {
                _isLevelCompleted = true;
                if (SAParticleEffect.Instance) SAParticleEffect.Instance.PlayParticle();
                _winCoroutine = StartCoroutine(OnWinCoroutine());
            }
        }
    
        public void OnLevelFailed()
        {
            foreach (var star in _starIcons) star.SetActive(false);
            _levelWinText.SetActive(false);
            _levelLoseText.SetActive(true);
            _restartButton.SetActive(true);
            _nextLevelButton.SetActive(false);
            _menuButton.SetActive(false);
            _isLevelCompleted = true;
            StartCoroutine(OnLoseCoroutine());
        }

        private IEnumerator OnLoseCoroutine()
        {
            if (_winCoroutine != null) StopCoroutine(_winCoroutine);
            yield return new WaitForSeconds(1f);
            _levelEndedPanel.SetActive(true);
            if (SAAudioManager.instance)
            {
                SAAudioManager.instance.Play("loss");
            }
        }

        private IEnumerator OnWinCoroutine()
        {
            yield return new WaitForSeconds(1.5f);
            _levelEndedPanel.SetActive(true);
            if (SAAudioManager.instance) SAAudioManager.instance.Play("WIn");
        }

        public void NextLevelButton()
        {
            int activeSceneIndex = SceneManager.GetActiveScene().buildIndex;
        
            if (activeSceneIndex >= SceneManager.sceneCountInBuildSettings - 1) SceneManager.LoadScene(0);
            else SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    
        public void Retry() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
