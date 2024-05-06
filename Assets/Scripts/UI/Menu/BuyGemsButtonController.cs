using System;
using Sounds;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Menu
{
    public class BuyGemsButtonController : MonoBehaviour
    {
        [SerializeField] private Button _thisButton;
        [SerializeField] private int _packIndex;
        [Inject] private IAPService _iapService;

        private void Start()
        {
            _thisButton.onClick.AddListener(OnButtonClick);
        }

        private void OnDestroy()
        {
            _thisButton.onClick.RemoveListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            SAAudioManager.instance.Play("Click");
            switch (_packIndex)
            {
                case 0:
                    _iapService.BuyPack1();
                    break;
                case 1:
                    _iapService.BuyPack2();
                    break;
                case 2:
                    _iapService.BuyPack3();
                    break;
                case 3:
                    _iapService.BuyPack4();
                    break;
                default:
                    throw new Exception("There is no pack with index " + _packIndex);
                    break;
            }
        }
    }
}