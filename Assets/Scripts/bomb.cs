using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bomb : MonoBehaviour
{
    public List<GameObject> botsTokill;
    public GameObject blasteffect;
    // Start is called before the first frame update
    void Start()
    {
        blasteffect.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            Destroy(collision.gameObject);
            KillThisBot();
        }
        if (collision.gameObject.tag == "SuperBullet")
        {
            KillThisBot();
        }
        if(collision.gameObject.tag=="Hit")
        {
            KillThisBot();
        }
    }

    public void KillThisBot()
    {
        blasteffect.transform.parent = null;
        blasteffect.SetActive(true);
      for(int i=0;i<botsTokill.Count;i++)
        {
            botsTokill[i].GetComponent<PlayerController>().KillThisBot();
        }
        Destroy(gameObject);
    }
}
