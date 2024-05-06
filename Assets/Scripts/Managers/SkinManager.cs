using System.Collections.Generic;
using Scripts.Gameplay.Managers;
using UnityEngine;

namespace Managers
{
    public class SkinManager : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _skins;
    
        private void Awake()
        {
            var skinIndex = PlayerPrefsManager.GetCurrentEquippedSkin();
            for (int i = 0; i < _skins.Count; i++) _skins[i].SetActive(i == skinIndex);
        }
    }
}
