using System;
using Scripts.Gameplay.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ShopButtonController : MonoBehaviour
    {
        private const string ADS_WATCHED_FORMAT = "{0}/{1}";
        
        [SerializeField] private GameObject _pricePanel;
        [SerializeField] private GameObject _adsPanel;
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private TextMeshProUGUI _adsWatchedText;
        [SerializeField] private RawImage _thisTexture;
        [SerializeField] private Button _thisButton;

        public void Initialize(int skinIndex, SkinModel skinModel, bool isUnlocked, bool isEquipped, RenderTexture skinTexture, Action<int> onSkinButtonClick)
        {
            _thisTexture.texture = skinTexture;
            _priceText.text = skinModel.Price.ToString();
            _pricePanel.SetActive(!isUnlocked && skinModel.WayToGetSkin != EWayToGetSkin.Ads);
            _adsPanel.SetActive(!isUnlocked && skinModel.WayToGetSkin == EWayToGetSkin.Ads);
            _adsWatchedText.text = String.Format(ADS_WATCHED_FORMAT, PlayerPrefsManager.GetSkinForAdsWatched(skinIndex), skinModel.AdsNeeded); 
            _thisButton.onClick.AddListener(() => onSkinButtonClick.Invoke(skinIndex));
        }

        private void OnDestroy() => _thisButton.onClick.RemoveAllListeners();
    }
}
