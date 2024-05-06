using System.Collections.Generic;
using Scripts.Gameplay.Managers;
using UnityEngine;
using Zenject.SpaceFighter;

namespace UI.Game
{
    public class StarsPanelController : MonoBehaviour
    {
        public static StarsPanelController Instance;

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
            if (!PlayerPrefsManager.HasStars(level) || PlayerPrefsManager.GetStars(level) < _currentStarAmount) PlayerPrefsManager.SetStars(level, _currentStarAmount);
        }

        public void DisableStars(List<GameObject> starIcons)
        {
            for (int i = 0; i < starIcons.Count; i++)
            {
                starIcons[i].SetActive(_starIcons[i].activeSelf);
            }
        }
    }
}
