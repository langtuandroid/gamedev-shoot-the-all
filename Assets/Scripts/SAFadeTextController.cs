using DG.Tweening;
using TMPro;
using UnityEngine;

public class SAFadeTextController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private float _fadeTime;

    private void Start()
    {
        _text.DOFade(0, 0);
        _text.DOFade(1, _fadeTime / 2).OnComplete(() => _text.DOFade(0, _fadeTime / 2).SetDelay(_fadeTime / 4)).SetDelay(_fadeTime / 4);
    }
}
