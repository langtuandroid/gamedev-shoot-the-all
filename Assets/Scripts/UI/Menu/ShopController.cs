using System;
using System.Collections.Generic;
using Integration;
using Scripts.Gameplay.Managers;
using Sounds;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class ShopController : MonoBehaviour
    {
        [SerializeField] private List<SkinModel> _skinModels;
        [SerializeField] private List<RenderTexture> _skinTextures;
        [SerializeField] private ShopButtonController _shopButtonPrefab;
        [SerializeField] private Transform _buttonsContainer;
        [SerializeField] private RawImage _mainSkinImage;
        [SerializeField] private TextMeshProUGUI _coinsText;
        [Inject] private AdMobController _adMobController;

        private List<ShopButtonController> _skinButtons = new();
        
        private static int CurrentSkinForAdsWatching = -1;
    
        void Start()
        {
            PlayerPrefsManager.ON_COINS_CHANGED += () => _coinsText.text = PlayerPrefsManager.GetPaidCurrency().ToString();
            _coinsText.text = PlayerPrefsManager.GetPaidCurrency().ToString();
            InitializeShop();
        }

        private void InitializeShop()
        {
            foreach (Transform child in _buttonsContainer) Destroy(child.gameObject);
            SetSkinUnlocked(0);
            for (int i = 0; i < _skinModels.Count; i++)
            {
                var buttonPrefab = Instantiate(_shopButtonPrefab, _buttonsContainer);
                buttonPrefab.Initialize(i, _skinModels[i], 1 == PlayerPrefsManager.GetIsSkinUnlocked(i), i == PlayerPrefsManager.GetCurrentEquippedSkin(), _skinTextures[i], OnSkinButtonClick);
                _skinButtons.Add(buttonPrefab);
            }

            var currentEquipped = PlayerPrefsManager.GetCurrentEquippedSkin();
            EquipSkin(currentEquipped);
            _mainSkinImage.texture = _skinTextures[currentEquipped];
        }
        
        private void OnSkinButtonClick(int skinIndex)
        {
            SAAudioManager.instance.Play("Click");
            var isSkinUnlocked = PlayerPrefsManager.GetIsSkinUnlocked(skinIndex) == 1;
            var skinModel = _skinModels[skinIndex];
                    
            if (isSkinUnlocked)
            {
                EquipSkin(skinIndex);
                InitializeShop();
                return;
            }
            
            switch (skinModel.WayToGetSkin)
            {
                case EWayToGetSkin.PaidMoney:
                    
                    if (skinModel.Price > PlayerPrefsManager.GetPaidCurrency()) return;

                    PlayerPrefsManager.AddPaidCurrency(-skinModel.Price);
                    _coinsText.text = PlayerPrefsManager.GetPaidCurrency().ToString();
                    SetSkinUnlocked(skinIndex);
                    EquipSkin(skinIndex);
                    InitializeShop();
                    break;
                
                case EWayToGetSkin.Ads:

                    CurrentSkinForAdsWatching = skinIndex;
                    _adMobController.ShowRewardedAd(OnRewardedAdClosed);
                    
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }

        private void OnRewardedAdClosed()
        {
            PlayerPrefsManager.AddSkinForAdsWatched(CurrentSkinForAdsWatching);

            if (_skinModels[CurrentSkinForAdsWatching].AdsNeeded <= PlayerPrefsManager.GetSkinForAdsWatched(CurrentSkinForAdsWatching))
            {
                SetSkinUnlocked(CurrentSkinForAdsWatching);
                EquipSkin(CurrentSkinForAdsWatching);
            }
            InitializeShop();
        }
        
        private void SetSkinUnlocked(int index) => PlayerPrefsManager.SetSkinUnlocked(index);

        private void EquipSkin(int index)
        {
            
            // for (int i = 0; i < _skinButtons.Count; i++)
            // {
            //     if (PlayerPrefsManager.GetIsSkinUnlocked(i) == 0) continue;
            //     _skinButtons[i].SetEquipped();
            // }

            PlayerPrefsManager.SetCurrentEquippedSkin(index);
        }
    }

    [Serializable] public class SkinModel
    {
        public int Price;
        public int AdsNeeded;
        public EWayToGetSkin WayToGetSkin;
    }
    
    public enum EWayToGetSkin
    {
        InGameMoney,
        PaidMoney,
        Ads
    }
}
