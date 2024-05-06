using Scripts.Gameplay.Managers;
using Sounds;
using TMPro;
using UnityEngine;

public class SAAudioButtonController : MonoBehaviour
{
    [SerializeField] private GameObject _audioDisabledImage;
    [SerializeField] private TextMeshProUGUI _audioText;
    
    private void Start()
    {
        _audioDisabledImage.SetActive(PlayerPrefsManager.GetAudio() != 0);
        _audioText.text = PlayerPrefsManager.GetAudio() == 0 ? "Sound On" : "Sound Off";
    }
    
    public void AudioButton()
    {
        PlayerPrefsManager.SetAudio(PlayerPrefsManager.GetAudio() == 0 ? 1 : 0);
        _audioDisabledImage.SetActive(PlayerPrefsManager.GetAudio() != 0);
        _audioText.text = PlayerPrefsManager.GetAudio() == 0 ? "Sound On" : "Sound Off";
        SAAudioManager.instance.Play("Click");
    }
}