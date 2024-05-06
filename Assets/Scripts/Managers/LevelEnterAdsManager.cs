using Integration;
using Zenject;

namespace Managers
{
    public class LevelEnterAdsManager
    {
        [Inject] private InterstitialAdController _interstitialAdController;
        [Inject] private IAPService _iapService;
        [Inject] private AdMobController _adMobController;

        private int _levelEnteredCount = 0;
        
        public void LevelLoaded()
        {
            _levelEnteredCount++;
            if (_levelEnteredCount % 2 == 0 && _interstitialAdController.ShowAd())
            {
                _interstitialAdController.OnAdClosed += OnAdClosed;
            }
            else if (_levelEnteredCount % 3 == 0)
            {
                _levelEnteredCount = 0;
                _iapService.ShowSubscriptionPanel();
                _interstitialAdController.LoadAd();
            }
            else
            {
                _adMobController.ShowBanner(true);
                _interstitialAdController.LoadAd();
            }
        }

        private void OnAdClosed()
        {
            _interstitialAdController.OnAdClosed -= OnAdClosed;
        }
    }
}