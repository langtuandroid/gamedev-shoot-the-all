using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public class SAPlayerController : MonoBehaviour
{
    [SerializeField] private ParticleSystem shootParticle;
    [SerializeField] private GameObject LeftShoulder, RightShoulder, Head;
    [SerializeField] private Transform Gun;
    [SerializeField] private Transform GunModel;
    [SerializeField] private Transform PlayerModel;
    [SerializeField] private bool SuperBullet;
    [SerializeField] private bool active;
    [SerializeField] private bool shoot;
    [SerializeField] private bool isLookAtGun;
    [SerializeField] private bool isXArmRotation;
    [SerializeField] private GameObject workingarm;
    [SerializeField] private float workingArmRotation = 0f;
    [SerializeField] private float gunAnimationOffset;
    [SerializeField] private Vector3 gunAnimationRotation;
    [SerializeField] private float bodyRotationMin = 20f;
    [SerializeField] private float bodyRotationMax = 160f;
    
    private bool _shootDelayPassed = true;
    private float x, y, z;
    private List<Vector3> _listOfPoints = new();
    private BulletController _currentBullet;
    private Collider[] _collidersInBot;
    private Rigidbody[] _rbInBot;
    private BoxCollider[] _boxCollidersInBot;

    void Start()
    {
        active = false;
        RemovePhysics();
        GetCurrentOffset();
        if (workingarm == null)
        {
            GetComponent<Animator>().enabled = true;
        }
        else
        {
            z += workingArmRotation;
            workingarm.transform.localRotation = isXArmRotation ? Quaternion.Euler(z, y, x) : Quaternion.Euler(x, y, z);
        }
    }
    
    void Update()
    {
        if (isLookAtGun)
        {
            LeftShoulder.transform.LookAt(Gun);
            RightShoulder.transform.LookAt(Gun);
            Head.transform.LookAt(Gun);
            Gun.transform.localRotation = Quaternion.Euler(0, 0, -z);
            PlayerModel.transform.localEulerAngles = new Vector3(0f, Math.Abs(Gun.transform.localRotation.eulerAngles.z) * 0.9f, 0f);


            var tempWorkingArmRotation = Math.Abs(workingArmRotation);

            if (tempWorkingArmRotation > 360)
            {
                if (tempWorkingArmRotation % 360f < 180)
                {
                    tempWorkingArmRotation %= 180f;
                }
                else
                {
                    tempWorkingArmRotation %= 360f;
                }
            }
            
            float yRotation;

            if (tempWorkingArmRotation is >= 0 and < 180)
            {
                yRotation = Mathf.Lerp(bodyRotationMin, bodyRotationMax, Mathf.InverseLerp(0f, 180f, tempWorkingArmRotation));
            }
            else
            {
                yRotation = Mathf.Lerp(bodyRotationMax, bodyRotationMin, Mathf.InverseLerp(180f, 360f, tempWorkingArmRotation));
            }

            PlayerModel.transform.localRotation = Quaternion.Euler(0f, yRotation, 0f);
        }

        if (active)
        {
            if (Input.GetMouseButton(0))
            {
                z += Input.GetAxis("Mouse X");
                z -= Input.GetAxis("Mouse Y");
                workingArmRotation += Input.GetAxis("Mouse X");
                workingArmRotation -= Input.GetAxis("Mouse Y");
                workingarm.transform.localRotation = isXArmRotation ? Quaternion.Euler(z, y, x) : Quaternion.Euler(x, y, z);
            }
        }
        
        if (shoot && _currentBullet == null)
        {
            active = true;
        }
    }

    public void Disable() => active = false;
    
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            OnBotDeath();
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("SuperBullet") || collision.gameObject.CompareTag("Hit"))
        {
            OnBotDeath();
        }
    }

    private void RemovePhysics()
    {
        _collidersInBot = GetComponentsInChildren<Collider>();
        _rbInBot = GetComponentsInChildren<Rigidbody>();
        _boxCollidersInBot = GetComponentsInChildren<BoxCollider>();
        
        foreach (var bc in _boxCollidersInBot)
            bc.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;

        foreach (var cl in _collidersInBot) cl.enabled = false;
        foreach (var rb in _rbInBot) rb.isKinematic = true;

        transform.GetComponent<CapsuleCollider>().enabled = true;
    }

    public void OnBotDeath()
    {
        shoot = false;
        active = false;
        foreach (var c in _collidersInBot)
        {
            c.enabled = true;
        }

        foreach (var rb in _rbInBot)
        {
            rb.isKinematic = false;
        }

        transform.GetComponent<CapsuleCollider>().enabled = false;
        if (gameObject.CompareTag("Emeny"))
        {
            tag = "Untagged";
            SAGameManager.Instance.OnBotKilled();
        }
        else if (gameObject.CompareTag("Player")) SAGameManager.Instance.LevelFail();

        Gun.GetComponent<SARaycastReflectionWorker>().enabled = false;
        Gun.GetComponent<LineRenderer>().enabled = false;
        GunModel.gameObject.SetActive(false);
        if (AudioManager.instance) AudioManager.instance.Play("Hit");
        enabled = false;
    }

    public bool Shoot()
    {
        if (active && _shootDelayPassed)
        {
            _shootDelayPassed = false;
            StartCoroutine(ShootDelay());
            SpawnBullet();
            return true;
        }

        return false;
    }

    private IEnumerator ShootDelay()
    {
        yield return new WaitForSeconds(2);
        _shootDelayPassed = true;
    }

    private void SpawnBullet()
    {
        shootParticle.Stop();
        shootParticle.Play();
        
        if (Gun.GetComponent<SARaycastReflectionWorker>().isKillableOnShot)
        {
            OnBotDeath();
            Destroy(_currentBullet);
            return;
        }
        _listOfPoints.Clear();
        Vector3[] temparray = new Vector3[Gun.GetComponent<LineRenderer>().positionCount];
        Gun.GetComponent<LineRenderer>().GetPositions(temparray);
        _listOfPoints = temparray.ToList();
        _currentBullet = Instantiate(SAGameManager.Instance.GetBullet(), _listOfPoints[0], Quaternion.identity);
        _currentBullet.SetPath(_listOfPoints);
        if (_listOfPoints.Count > 1)
        {
            _currentBullet.transform.LookAt(_listOfPoints[1]);
        }
        shoot = true;
        if (SuperBullet)
        {
            _currentBullet.tag = "SuperBullet";
        }

        if (AudioManager.instance)
        {
            AudioManager.instance.Play("Bullet");
        }
        GunAnimation();
    }

    private void GunAnimation()
    {
        Gun.DOBlendableLocalMoveBy(Gun.right * -gunAnimationOffset, 0.1f);
        Gun.DOBlendableRotateBy(gunAnimationRotation, 0.1f).OnComplete(() =>
        {
            Gun.DOBlendableLocalMoveBy(Gun.right * gunAnimationOffset, 0.2f);
            Gun.DOBlendableRotateBy(-gunAnimationRotation, 0.2f);
        });
    }
    
    public void BotWin()
    {
        Gun.GetComponent<SARaycastReflectionWorker>().enabled = false;
        Gun.GetComponent<LineRenderer>().enabled = false;
        active = false;

    }

    private void GetCurrentOffset()
    {
        if (isXArmRotation)
        {
            x = workingarm.transform.localEulerAngles.z;
            y = workingarm.transform.localEulerAngles.y;
            z = workingarm.transform.localEulerAngles.x;
        }
        else
        {
            x = workingarm.transform.localEulerAngles.x;
            y = workingarm.transform.localEulerAngles.y;
            z = workingarm.transform.localEulerAngles.z;
        }

        active = true;
    }
}
