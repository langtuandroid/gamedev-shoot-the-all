using TMPro;
using UnityEngine;

public class SAAudioButtonController : MonoBehaviour
{
    [SerializeField] private GameObject _audioDisabledImage;
    [SerializeField] private TextMeshProUGUI _audioText;
    
    private void Start()
    {
        _audioDisabledImage.SetActive(PlayerPrefs.GetInt("Audio", 0) != 0);
        _audioText.text = PlayerPrefs.GetInt("Audio", 0) == 0 ? "Sound On" : "Sound Off";
    }
    
    public void AudioButton()
    {
        PlayerPrefs.SetInt("Audio", PlayerPrefs.GetInt("Audio", 0) == 0 ? 1 : 0);
        _audioDisabledImage.SetActive(PlayerPrefs.GetInt("Audio", 0) != 0);
        _audioText.text = PlayerPrefs.GetInt("Audio", 0) == 0 ? "Sound On" : "Sound Off";
        AudioManager.instance.Play("Click");
    }
}