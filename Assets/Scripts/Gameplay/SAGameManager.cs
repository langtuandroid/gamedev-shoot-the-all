using System.Collections;
using UI.Game;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Gameplay
{
    public class SAGameManager : MonoBehaviour
    {
        public static SAGameManager Instance;
        private const float DistanceToShoot = 0.003f;
    
        [SerializeField] private SABulletController _bulletPrefab;
        
        private Vector3 _firstMousePoint = Vector3.positiveInfinity;
        private int _amountOfBotsToKill;
        private bool _isEndGame;
        private bool _shootDelayPassed = false;
        private float _timerTime;
    
        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
            }
        }

        public void Start()
        {
            _amountOfBotsToKill = 0;
            SAPlayerController[] playerControllers = FindObjectsOfType<SAPlayerController>();
            foreach (var playerController in playerControllers)
            {
                if (playerController.transform.CompareTag("Emeny")) _amountOfBotsToKill++;
            }
            StartCoroutine(ShootDelay());
        }

        private IEnumerator ShootDelay()
        {
            yield return new WaitForSeconds(0.5f);
            _shootDelayPassed = true;
        }
        
        public void Update()
        {
            if (_isEndGame || !_shootDelayPassed) return;
            if (Input.GetMouseButton(0) && !EventSystem.current.currentSelectedGameObject)
            {
                _firstMousePoint = Input.mousePosition;
                _timerTime += Time.deltaTime;
            }

            if (Input.GetMouseButtonUp(0) && !EventSystem.current.currentSelectedGameObject)
            {
                if (_timerTime < 0.25f && Vector3.Distance(_firstMousePoint, Input.mousePosition) < DistanceToShoot) Shoot();

                _firstMousePoint = Vector3.positiveInfinity;
                _timerTime = 0;
            }
        }

        public void OnBotKilledAction()
        {
            if (_isEndGame) return;
            _amountOfBotsToKill--;
            if (_amountOfBotsToKill == 0)
            {
                _isEndGame = true;
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
            foreach (var playerController in pc)
            {
                if (!playerController.Shoot()) isShot = false;
            }
            if(isShot) StarsPanelController.Instance.StarLost();
        }

        public void LevelFailedAction()
        {
            _isEndGame = true;
            SAPlayerController[] pc = FindObjectsOfType<SAPlayerController>();
            foreach (var player in pc)
            {
                player.BotWin();
                player.GetComponent<SAPlayerController>().Disable();
                player.GetComponent<SAPlayerController>().enabled = false;
            }
            SAUiManager.Instance.OnLevelFailed();
        }

        public SABulletController GetBullet() => _bulletPrefab;
    }
}
