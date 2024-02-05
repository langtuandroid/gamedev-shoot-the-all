using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    public ParticleSystem shootParticle;
    public GameObject LeftShoulder, RightShoulder, Head;
    public Transform Gun;
    public Transform PlayerModel;
    public bool SuperBullet;
    public bool active;
    public bool shoot;
    public bool isLookAtGun;
    public bool isXArmRotation;
    private float xxx;
    private float x, y;
    private int i;
    public GameObject workingarm;
    private GameObject bullet;
    public List<Vector3> ListOfPoints;
    private Collider[] incolliders;
    private Rigidbody[] rigidbodies;
    private BoxCollider[] boxColliders;
    public float workingArmRotation = 0f;
    public float gunAnimationOffset;
    public Vector3 gunAnimationRotation;

    void Start()
    {
        i = 0;
        active = false;
        RemoveRigAndCol();
        XandYofPresent(workingarm);
        if (workingarm == null)
        {
            // RefBotPositions();
            GetComponent<Animator>().enabled = true;
        }
        else
        {
            xxx += workingArmRotation;
            workingarm.transform.localRotation = isXArmRotation ? Quaternion.Euler(xxx, y, x) : Quaternion.Euler(x, y, xxx);
        }
    }

    float yRotationMin = 20f;
    float yRotationMax = 160f;
    
    void Update()
    {
        if (isLookAtGun)
        {
            LeftShoulder.transform.LookAt(Gun);
            RightShoulder.transform.LookAt(Gun);
            Head.transform.LookAt(Gun);
            Gun.transform.localRotation = Quaternion.Euler(0, 0, -xxx);
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
                yRotation = Mathf.Lerp(yRotationMin, yRotationMax, Mathf.InverseLerp(0f, 180f, tempWorkingArmRotation));
            }
            else
            {
                yRotation = Mathf.Lerp(yRotationMax, yRotationMin, Mathf.InverseLerp(180f, 360f, tempWorkingArmRotation));
            }

            // Debug.Log("new rotation: " + yRotation);
            // Применение вращения ко второму объекту
            PlayerModel.transform.localRotation = Quaternion.Euler(0f, yRotation, 0f);
        }

        if (active)
        {
            if (Input.GetMouseButton(0))
            {
                xxx += Input.GetAxis("Mouse X");
                xxx -= Input.GetAxis("Mouse Y");
                workingArmRotation += Input.GetAxis("Mouse X");
                workingArmRotation -= Input.GetAxis("Mouse Y");
                // if (workingArmRotation < 0)
                // {
                //     workingArmRotation += 360f;
                // }
                workingarm.transform.localRotation = isXArmRotation ? Quaternion.Euler(xxx, y, x) : Quaternion.Euler(x, y, xxx);
            }
        }

        if (shoot && bullet != null)
        {
            active = false;
            bullet.transform.position =
                Vector3.MoveTowards(bullet.transform.position, ListOfPoints[i], 10 * Time.deltaTime);
            
            bullet.transform.LookAt(ListOfPoints[i]);
            if (bullet.transform.position == ListOfPoints[i])
            {
                if (i < ListOfPoints.Count - 1)
                {
                    i++;
                }
                else
                {
                    active = true;
                    Destroy(bullet, 0.1f);
                    shoot = false;
                }
            }
        }
        else if (shoot && bullet == null)
        {
            active = true;
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            KillThisBot();
            Destroy(bullet);
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("SuperBullet") || collision.gameObject.CompareTag("Hit"))
        {
            KillThisBot();
        }
    }

    private void RemoveRigAndCol()
    {
        incolliders = GetComponentsInChildren<Collider>();
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        boxColliders = GetComponentsInChildren<BoxCollider>();
        
        foreach (var bc in boxColliders)
            bc.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;

        foreach (var cl in incolliders) cl.enabled = false;
        foreach (var rb in rigidbodies) rb.isKinematic = true;

        transform.GetComponent<CapsuleCollider>().enabled = true;
    }

    public void KillThisBot()
    {
        shoot = false;
        foreach (var c in incolliders)
        {
            c.enabled = true;
        }

        foreach (var rb in rigidbodies)
        {
            rb.isKinematic = false;
        }

        transform.GetComponent<CapsuleCollider>().enabled = false;
        // GetComponent<Animator>().enabled = false;
        if (gameObject.CompareTag("Emeny"))
        {
            tag = "Untagged";
            GameManager.Instance.killcount();
        }
        else if (gameObject.CompareTag("Player")) UiManager.Instance.LevelFail();

        Gun.gameObject.SetActive(false);
        if (AudioManager.instance) AudioManager.instance.Play("Hit");
    }

    public void Shoot()
    {
        if (active) SendBullet();
    }

    private void SendBullet()
    {
        GunAnimation();
        shootParticle.Stop();
        shootParticle.Play();
        i = 0;
        ListOfPoints.Clear();
        Vector3[] temparray = new Vector3[Gun.GetComponent<LineRenderer>().positionCount];
        Gun.GetComponent<LineRenderer>().GetPositions(temparray);
        ListOfPoints = temparray.ToList();
        bullet = Instantiate(GameManager.Instance.Bullet, ListOfPoints[0], Quaternion.identity);
        bullet.transform.LookAt(ListOfPoints[1]);
        shoot = true;
        if (SuperBullet)
        {
            bullet.tag = "SuperBullet";
        }

        if (AudioManager.instance)
        {
            AudioManager.instance.Play("Bullet");
        }
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
    
    public void DanceForWin()
    {
        // RefBotPositions();
        // GetComponent<Animator>().enabled = true;
        Gun.GetComponent<RaycastReflection>().enabled = false;
        Gun.GetComponent<LineRenderer>().enabled = false;
        Gun.gameObject.SetActive(false);
        if (bullet != null) Destroy(bullet);
        active = false;

    }

    public void XandYofPresent(GameObject workingarm)
    {
        if (isXArmRotation)
        {
            x = workingarm.transform.localEulerAngles.z;
            y = workingarm.transform.localEulerAngles.y;
            xxx = workingarm.transform.localEulerAngles.x;
        }
        else
        {
            x = workingarm.transform.localEulerAngles.x;
            y = workingarm.transform.localEulerAngles.y;
            xxx = workingarm.transform.localEulerAngles.z;
        }

        active = true;
    }

    // public void RefBotPositions()
    // {
    //     GetComponent<Animator>().enabled = false;
    //     if (GameManager.Instance)
    //     {
    //         LLL = GameManager.Instance.LLL;
    //         RRR = GameManager.Instance.RRR;
    //     }
    //
    //     leftArm.name = LLL.transform.GetChild(0).name;
    //     RightArm.name = RRR.transform.GetChild(0).name;
    //     GameObject lll = leftArm.transform.parent.gameObject;
    //     GameObject rrr = RightArm.transform.parent.gameObject;
    //     lll.transform.localPosition = LLL.transform.localPosition;
    //     lll.transform.localEulerAngles = LLL.transform.localEulerAngles;
    //     lll.transform.GetChild(0).transform.localPosition = LLL.transform.GetChild(0).transform.localPosition;
    //     lll.transform.GetChild(0).transform.localEulerAngles = LLL.transform.GetChild(0).transform.localEulerAngles;
    //     lll.transform.GetChild(0).GetChild(0).transform.localPosition =
    //         LLL.transform.GetChild(0).GetChild(0).transform.localPosition;
    //     lll.transform.GetChild(0).GetChild(0).transform.localEulerAngles =
    //         LLL.transform.GetChild(0).GetChild(0).transform.localEulerAngles;
    //     rrr.transform.localPosition = RRR.transform.localPosition;
    //     rrr.transform.localEulerAngles = RRR.transform.localEulerAngles;
    //     rrr.transform.GetChild(0).transform.localPosition = RRR.transform.GetChild(0).transform.localPosition;
    //     rrr.transform.GetChild(0).transform.localEulerAngles = RRR.transform.GetChild(0).transform.localEulerAngles;
    //     rrr.transform.GetChild(0).GetChild(0).transform.localPosition =
    //         RRR.transform.GetChild(0).GetChild(0).transform.localPosition;
    //     rrr.transform.GetChild(0).GetChild(0).transform.localEulerAngles =
    //         RRR.transform.GetChild(0).GetChild(0).transform.localEulerAngles;
    // }
}
