using System.Collections.Generic;
using UnityEngine;

public class SAExplosiveBomb : MonoBehaviour
{
    [SerializeField] private List<GameObject> botsTokill;
    [SerializeField] private GameObject blasteffect;
    
    void Start()
    {
        blasteffect.SetActive(false);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
            KillThisBot();
        }
        if (collision.gameObject.CompareTag("SuperBullet") || collision.gameObject.CompareTag("Hit")) KillThisBot();
    }

    private void KillThisBot()
    {
        blasteffect.transform.parent = null;
        blasteffect.SetActive(true);
        foreach (var bot in botsTokill) bot.GetComponent<SAPlayerController>().OnBotDeath();
        Destroy(gameObject);
    }
}
