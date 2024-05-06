using System.Collections.Generic;
using UnityEngine;

namespace UI.Menu
{
    public class SABGWorker : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _phoneBGs;
        [SerializeField] private List<GameObject> _tabletBGs;
        [SerializeField] private bool _overridePhone;

        void Start()
        {
            var isPhone = _overridePhone || CheckDeviceType();
            foreach (var phoneBG in _phoneBGs) phoneBG.SetActive(isPhone);
            foreach (var tabletBG in _tabletBGs) tabletBG.SetActive(!isPhone);
        }

        private bool CheckDeviceType()
        {
            float screenSizeInches = Mathf.Sqrt(Mathf.Pow(Screen.width / Screen.dpi, 2) + Mathf.Pow(Screen.height / Screen.dpi, 2));
            return !(screenSizeInches >= 7.0f);
        }
    }
}
