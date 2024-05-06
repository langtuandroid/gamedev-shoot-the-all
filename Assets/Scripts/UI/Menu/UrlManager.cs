using System;
using System.Collections;
using System.Threading.Tasks;
using Sounds;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu
{
    public class UrlManager : MonoBehaviour
    {
        [SerializeField] private string _urlForPrivacyPolicy;
        [SerializeField] private string _urlForTermsOfUse;
        [SerializeField] private string _urlForPrivacyPolicyIos;
        [SerializeField] private string _urlForTermsOfUseIos;
        [SerializeField] private Button _termsButton;
        [SerializeField] private Button _privacyButton;

        private bool _externalOpeningUrlDelayFlag;

        private void Awake()
        {
#if UNITY_IOS
           
            if (_termsButton != null)
                _termsButton.onClick.AddListener(() => OpenUrl(_urlForTermsOfUseIos));

            _privacyButton.onClick.AddListener(() => OpenUrl(_urlForPrivacyPolicyIos));
#else
            if (_termsButton != null)
                _termsButton.onClick.AddListener(() => OpenUrl(_urlForTermsOfUse));

            if (_privacyButton != null)
                _privacyButton.onClick.AddListener(() => OpenUrl(_urlForPrivacyPolicy));
#endif
        }

        private void OnDestroy()
        {
            if (_termsButton != null)
                _termsButton.onClick.RemoveListener(() => OpenUrl(_urlForTermsOfUse));

            if (_privacyButton != null)
                _privacyButton.onClick.RemoveListener(() => OpenUrl(_urlForPrivacyPolicy));
        }

        private async void OpenUrl(string url)
        {
            SAAudioManager.instance.Play("Click");
            if (_externalOpeningUrlDelayFlag) return;
            _externalOpeningUrlDelayFlag = true;
            await OpenURLAsync(url);
            StartCoroutine(WaitForSeconds(1, () => _externalOpeningUrlDelayFlag = false));
        }
    
        private async Task OpenURLAsync(string url)
        {
            await Task.Delay(1);
            try
            {
                Application.OpenURL(url);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error while opening a url: {url}: {e.Message}");
            }
        }

        private IEnumerator WaitForSeconds(float seconds, Action callback)
        {
            yield return new WaitForSeconds(seconds);
            callback?.Invoke();
        } 
    }
}