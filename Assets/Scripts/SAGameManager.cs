using UnityEngine;
using UnityEngine.EventSystems;

public class SAGameManager : MonoBehaviour
{
    private const float DistanceToShoot = 0.003f;
    
    [SerializeField] private BulletController Bullet;
    public static SAGameManager Instance;
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
        SAPlayerController[] pc = FindObjectsOfType<SAPlayerController>();
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
        if (isEndGame) return;
        _numberOfBotsToKill--;
        if (_numberOfBotsToKill == 0)
        {
            isEndGame = true;
            SAPlayerController[] pc = FindObjectsOfType<SAPlayerController>();
            for (int i = 0; i < pc.Length; i++)
            {
                if (pc[i].transform.tag == "Player")
                {
                    pc[i].BotWin();
                    pc[i].GetComponent<SAPlayerController>().Disable();
                    pc[i].GetComponent<SAPlayerController>().enabled = false;
                }
            }
            SAUiManager.Instance.OnLevelCompleted();
        }
    }

    private void Shoot()
    {
        
        SAPlayerController[] pc = FindObjectsOfType<SAPlayerController>();
        bool isShot = true;
        for (int i = 0; i < pc.Length; i++)
        {
            if (!pc[i].Shoot()) isShot = false;
        }
        if(isShot) StarsPanelController.Instance.StarLost();
    }

    public void LevelFail()
    {
        isEndGame = true;
        SAPlayerController[] pc = FindObjectsOfType<SAPlayerController>();
        foreach (var t in pc)
        {
            t.BotWin();
            t.GetComponent<SAPlayerController>().Disable();
            t.GetComponent<SAPlayerController>().enabled = false;
        }
        SAUiManager.Instance.LevelFail();
    }

    public BulletController GetBullet() => Bullet;
}
