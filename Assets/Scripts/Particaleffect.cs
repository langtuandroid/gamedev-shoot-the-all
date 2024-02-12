using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Particaleffect : MonoBehaviour
{
    public static Particaleffect instance;

    void Awake()
    {
        if (instance != null) Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    
    public void PlayParticle() => GetComponent<ParticleSystem>().Play();
}
