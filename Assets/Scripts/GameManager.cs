using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    private const float DistanceToShoot = 0.003f;
    
    [SerializeField] private BulletController Bullet;
    public static GameManager Instance;
    private int _numberOfBotsToKill;
    private bool isEndGame = false;

    private float timeCount;
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
            timeCount += Time.deltaTime;
        }

        if (Input.GetMouseButtonUp(0) && !EventSystem.current.currentSelectedGameObject)
        {
            if (timeCount < 0.25f && Vector3.Distance(firstMousePoint, Input.mousePosition) < DistanceToShoot)
            {
                Shoot();
            }
            
            firstMousePoint = Vector3.positiveInfinity;
            timeCount = 0;
        }
    }

    public void Start()
    {
        _numberOfBotsToKill = 0;
        PlayerController[] pc = FindObjectsOfType<PlayerController>();
        for (int i = 0; i < pc.Length; i++)
        {
            if (pc[i].transform.tag == "Emeny")
            {
                _numberOfBotsToKill++;
            }
        }
    }

    public void OnBotKilled()
    {
        _numberOfBotsToKill--;
        if (_numberOfBotsToKill == 0)
        {
            isEndGame = true;
            PlayerController[] pc = FindObjectsOfType<PlayerController>();
            for (int i = 0; i < pc.Length; i++)
            {
                if (pc[i].transform.tag == "Player")
                {
                    pc[i].BotWin();
                    pc[i].GetComponent<PlayerController>().Disable();
                    pc[i].GetComponent<PlayerController>().enabled = false;
                }
            }
            UiManager.Instance.OnLevelCompleted();
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
            t.BotWin();
            t.GetComponent<PlayerController>().Disable();
            t.GetComponent<PlayerController>().enabled = false;
        }
        UiManager.Instance.LevelFail();
    }

    public BulletController GetBullet() => Bullet;
}
