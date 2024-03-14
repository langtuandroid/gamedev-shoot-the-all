using UnityEngine;

public class SAParticleEffect : MonoBehaviour
{
    public static SAParticleEffect Instance;

    void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    
    public void PlayParticle() => GetComponent<ParticleSystem>().Play();
}
