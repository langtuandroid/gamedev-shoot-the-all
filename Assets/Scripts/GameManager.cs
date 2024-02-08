using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    private const float DistanceToShoot = 0.003f;
    
    public BulletController Bullet;
    public static GameManager Instance;
    public int NumberOfBotsToKill;
    public float timecount;
    public GameObject LLL, RRR;
    private bool isEndGame = false;

    private Vector3 firstMousePoint = Vector3.positiveInfinity;
    
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
    }

    public void Update()
    {
        if (isEndGame) return;
        if (Input.GetMouseButton(0) && !EventSystem.current.currentSelectedGameObject)
        {
            firstMousePoint = Input.mousePosition;
            timecount += Time.deltaTime;
        }

        if (Input.GetMouseButtonUp(0) && !EventSystem.current.currentSelectedGameObject)
        {
            if (timecount < 0.25f && Vector3.Distance(firstMousePoint, Input.mousePosition) < DistanceToShoot)
            {
                Shoot();
            }
            
            firstMousePoint = Vector3.positiveInfinity;
            timecount = 0;
        }
    }

    public void Start()
    {
        NumberOfBotsToKill = 0;
        PlayerController[] pc = FindObjectsOfType<PlayerController>();
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
            isEndGame = true;
            PlayerController[] pc = FindObjectsOfType<PlayerController>();
            for (int i = 0; i < pc.Length; i++)
            {
                if (pc[i].transform.tag == "Player")
                {
                    pc[i].DanceForWin();
                    pc[i].GetComponent<PlayerController>().active = false;
                    pc[i].GetComponent<PlayerController>().enabled = false;
                }
            }
            UiManager.Instance.LevelCompleted();
        }
    }

    private void Shoot()
    {
        PlayerController[] pc = FindObjectsOfType<PlayerController>();
        for (int i = 0; i < pc.Length; i++)
        {
            pc[i].Shoot();
        }
    }

    public void LevelFail()
    {
        isEndGame = true;
        PlayerController[] pc = FindObjectsOfType<PlayerController>();
        foreach (var t in pc)
        {
            t.DanceForWin();
            t.GetComponent<PlayerController>().active = false;
            t.GetComponent<PlayerController>().enabled = false;
        }
        UiManager.Instance.LevelFail();
    }
}
