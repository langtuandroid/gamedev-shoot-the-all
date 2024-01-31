using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public GameObject Bullet;
    public static GameManager Instance;
    public int NumberOfBotsToKill;
    public float timecount;
    public GameObject LLL, RRR;
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
    }
    public void Update()
    {
        if (Input.GetMouseButton(0)&&!EventSystem.current.currentSelectedGameObject)
        {
            timecount += Time.deltaTime;
        }
        if (Input.GetMouseButtonUp(0)&& !EventSystem.current.currentSelectedGameObject)
        {
         
            if (timecount < 0.25f)
            {
                shoot();
            }
            timecount = 0;
        }
    }
    public void Start()
    {
        NumberOfBotsToKill = 0;
       PlayerController[] pc = GameObject.FindObjectsOfType<PlayerController>();
        for (int i = 0; i < pc.Length; i++)
        {
            if (pc[i].transform.tag == "Emeny")
            {
                NumberOfBotsToKill++;
            }
        }
    }
    public void killcount()
    {
        NumberOfBotsToKill--;
        if (NumberOfBotsToKill == 0)
        {
            UiManager.Instance.levelCompleted();
            PlayerController[] pc = GameObject.FindObjectsOfType<PlayerController>();
            for (int i=0;i<pc.Length;i++)
            {
               if(pc[i].transform.tag=="Player")
                {
                    pc[i].DanceForWin();
                    pc[i].GetComponent<PlayerController>().enabled = false;
                }
            }
        }
    }

    public void shoot()
    {
        PlayerController[] pc = FindObjectsOfType<PlayerController>();
        for(int i=0;i<pc.Length;i++)
        {
            Debug.Log(pc[i].transform.name);
            pc[i].Shoot();
        }
    }
}
