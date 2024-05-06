using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

namespace UI.Menu
{
    public class SARedirectButton : MonoBehaviour
    {
        [SerializeField] private string _link;
        [SerializeField] private List<Button> _buttonsToDisable;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(() =>
            {
                foreach (var button in _buttonsToDisable) button.interactable = false;
                OpenURLAndEnableButtonAsync();
            });
        }
        
        private async void OpenURLAndEnableButtonAsync()
        {
            await OpenURL(_link);
            foreach (var button in _buttonsToDisable) button.interactable = true;
        }
    
        private async Task OpenURL(string url)
        {
            try
            {
                Application.OpenURL(url);
            }
            catch (Exception e)
            {
                Debug.LogWarning("can't open the link");
            }
            await Task.Delay(1000);
        }
    }
}