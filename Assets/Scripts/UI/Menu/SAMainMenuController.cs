using System.Collections.Generic;
using Integration;
using Managers;
using Sounds;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Menu
{
    public class SAMainMenuController : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _backToMainMenuButtons;
        [SerializeField] private GameObject _mainMenuPanel;
        [SerializeField] private GameObject _levelsPanel;
        [SerializeField] private GameObject _settingsPanel;
        [SerializeField] private GameObject _shopPanel;
        [SerializeField] private List<Button> _gemShopButtons;
        [SerializeField] private List<Button> _gemShopBackButtons;
        [SerializeField] private List<GameObject> _gemShops;
        [Inject] private AdMobController _adMobController;

        private void Start()
        {
            foreach (var gemShopButton in _gemShopButtons)
            {
                gemShopButton.onClick.AddListener(() =>
                {
                    SAAudioManager.instance.Play("Click");
                    SetMenuButtonsActivity(false);
                    SetGemShopsActivity(true);
                });
            }
            foreach (var gemShopBackButton in _gemShopBackButtons)
            {
                gemShopBackButton.onClick.AddListener(() =>
                {
                    SAAudioManager.instance.Play("Click");
                    SetMenuButtonsActivity(true);
                    SetGemShopsActivity(false);
                });
            }
            SetMenuButtonsActivity(false);
            _levelsPanel.SetActive(false);
            _settingsPanel.SetActive(false);
            _shopPanel.SetActive(false);
            _mainMenuPanel.SetActive(true);
        }

        private void SetGemShopsActivity(bool isActive)
        {
            foreach (var gemShop in _gemShops) gemShop.SetActive(isActive);
        }
        
        private void OnDestroy()
        {
            foreach (var gemShopButton in _gemShopButtons) gemShopButton.onClick.RemoveAllListeners();
            foreach (var gemShopBackButton in _gemShopBackButtons) gemShopBackButton.onClick.RemoveAllListeners();
        }

        private void SetMenuButtonsActivity(bool state)
        {
            foreach (var button in _backToMainMenuButtons) button.SetActive(state);
        }
    
        public void SettingsButton()
        {
            SetMenuButtonsActivity(true);
            _mainMenuPanel.SetActive(false);
            _settingsPanel.SetActive(true);
            _levelsPanel.SetActive(false);
            _shopPanel.SetActive(false);
            SAAudioManager.instance.Play("Click");
        }

        public void PlayButton()
        {
            SetMenuButtonsActivity(true);
            _levelsPanel.SetActive(true);
            _mainMenuPanel.SetActive(false);
            _settingsPanel.SetActive(false);
            _shopPanel.SetActive(false);
            SAAudioManager.instance.Play("Click");
        }

        public void ShopButton()
        {
            SetMenuButtonsActivity(true);
            _levelsPanel.SetActive(false);
            _shopPanel.SetActive(true);
            _mainMenuPanel.SetActive(false);
            _settingsPanel.SetActive(false);
            SAAudioManager.instance.Play("Click");
        }
    
        public void BackToMainMenuButton()
        {
            SetMenuButtonsActivity(false);
            _levelsPanel.SetActive(false);
            _settingsPanel.SetActive(false);
            _shopPanel.SetActive(false);
            _mainMenuPanel.SetActive(true);
            SAAudioManager.instance.Play("Click");
        }
    }
}
