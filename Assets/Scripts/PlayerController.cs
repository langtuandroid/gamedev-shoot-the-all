using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class PlayerController : MonoBehaviour
{
    public GameObject leftArm, RightArm;
    public GameObject LeftGun, RightGun;
    public GameObject LLL;
    public GameObject RRR;
    public bool leftArmWork, RightArmWork, SuperBullet;
    public bool active;
    public bool shoot;
    private float mousex, xxx;
    private float x, y;
    private int i;
    public GameObject workingarm;
    private GameObject bullet;
    public List<Vector3> ListOfPoints;
    private Collider[] incolliders;
    private Rigidbody[] rigidbodies;
    private BoxCollider[] boxColliders;
    void Start()
    {
        i = 0;
        active = false;
        removerigandcol();
        if (leftArmWork)
        {
            LeftGun.SetActive(true);
            RightGun.SetActive(false);
            workingarm = leftArm;
            XandYofPresent(workingarm);
        }
        else if (RightArmWork)
        {
            RightGun.SetActive(true);
            LeftGun.SetActive(false);
            workingarm = RightArm;
            XandYofPresent(workingarm);
        }
        else
        {
            refbotpostions();
            RightGun.SetActive(false);
            LeftGun.SetActive(false);
            GetComponent<Animator>().enabled = true;
        }
        if(AdManager.instance)
        {
            AdManager.instance.loadInterstitial();
            AdManager.instance.showBannerAd();
        }
    }
    void Update()
    {
        if (active)
        {
            if (Input.GetMouseButton(0))
            {
                mousex = Input.GetAxis("Mouse X");
                xxx += mousex;
                workingarm.transform.localRotation = Quaternion.Euler(x, y, xxx);
            }
        }
        if (shoot && bullet != null)
        {
            active = false;
            bullet.transform.position = Vector3.MoveTowards(bullet.transform.position, ListOfPoints[i], 10 * Time.deltaTime);
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
        if (shoot && bullet == null)
        {
            active = true;
        }
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            KillThisBot();
            Destroy(bullet);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "SuperBullet")
        {
            KillThisBot();
        }
        if (collision.gameObject.tag == "Hit")
        {
            KillThisBot();
        }
    }
    public void removerigandcol()
    {
        GetComponent<Animator>().enabled = false;
        incolliders = GetComponentsInChildren<Collider>();
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        boxColliders = GetComponentsInChildren<BoxCollider>();
        for (int i = 0; i < boxColliders.Length; i++)
        {
            boxColliders[i].transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ;
        }
        for (int i = 0; i < incolliders.Length; i++)
        {
            incolliders[i].enabled = false;
        }
        for (int i = 0; i < rigidbodies.Length; i++)
        {
            rigidbodies[i].isKinematic = true;
        }
        CapsuleCollider cc = GetComponent<CapsuleCollider>();
        cc.enabled = true;
        cc.height = 1.85f;
        cc.center = new Vector3(0, 0.85f, 0);
    }
    public void KillThisBot()
    {
        for (int i = 0; i < incolliders.Length; i++)
        {
            incolliders[i].enabled = true;
        }
        for (int i = 0; i < rigidbodies.Length; i++)
        {
            rigidbodies[i].isKinematic = false;
        }
        transform.GetComponent<CapsuleCollider>().enabled = false;
        GetComponent<Animator>().enabled = false;
        if (gameObject.tag == "Emeny")
        {
            tag = "Untagged";
            GameManager.Instance.killcount();
        }
        else if(gameObject.tag=="Player")
        {
            UiManager.Instance.LevelFail();
        }
        LeftGun.SetActive(false);
        RightGun.SetActive(false);
        Destroy(GetComponent<PlayerController>());
        if(AudioManager.instance)
        {
            AudioManager.instance.Play("Hit");
        }
    }
    public void Shoot()
    {
        if (RightArmWork && active)
        {
            sendbullet(RightGun);
        }
        if (leftArmWork && active)
        {
            sendbullet(LeftGun);
        }
    }
    public void sendbullet(GameObject gun)
    {
        i = 0;
        ListOfPoints.Clear();
        Vector3[] temparray = new Vector3[gun.GetComponent<LineRenderer>().positionCount];
        gun.GetComponent<LineRenderer>().GetPositions(temparray);
        ListOfPoints = temparray.ToList();
        bullet = Instantiate(GameManager.Instance.Bullet, ListOfPoints[0], Quaternion.identity);
        shoot = true;
        if (SuperBullet)
        {
            bullet.tag = "SuperBullet";
        }
        if(AudioManager.instance)
        {
            AudioManager.instance.Play("Bullet");
        }
    }
    public void DanceForWin()
    {
        refbotpostions();
        GetComponent<Animator>().enabled = true;
//GetComponent<Animator>().SetTrigger("Dance");
        RightGun.GetComponent<RaycastReflection>().enabled = false;
        RightGun.GetComponent<LineRenderer>().enabled = false;
        LeftGun.SetActive(false);
        RightGun.SetActive(false);
        active = false;

    }
    public void XandYofPresent(GameObject workingarm)
    {
        x = workingarm.transform.localEulerAngles.x;
        y = workingarm.transform.localEulerAngles.y;
        xxx = workingarm.transform.localEulerAngles.z;
        active = true;
    }
    public void refbotpostions()
    {
        GetComponent<Animator>().enabled = false;
        if (GameManager.Instance)
        {
            LLL = GameManager.Instance.LLL;
            RRR = GameManager.Instance.RRR;
        }
        leftArm.name = LLL.transform.GetChild(0).name;
        RightArm.name = RRR.transform.GetChild(0).name;
        GameObject lll = leftArm.transform.parent.gameObject;
        GameObject rrr = RightArm.transform.parent.gameObject;
        lll.transform.localPosition = LLL.transform.localPosition;
        lll.transform.localEulerAngles = LLL.transform.localEulerAngles;
        lll.transform.GetChild(0).transform.localPosition = LLL.transform.GetChild(0).transform.localPosition;
        lll.transform.GetChild(0).transform.localEulerAngles = LLL.transform.GetChild(0).transform.localEulerAngles;
        lll.transform.GetChild(0).GetChild(0).transform.localPosition =
            LLL.transform.GetChild(0).GetChild(0).transform.localPosition;
        lll.transform.GetChild(0).GetChild(0).transform.localEulerAngles =
            LLL.transform.GetChild(0).GetChild(0).transform.localEulerAngles;
        rrr.transform.localPosition = RRR.transform.localPosition;
        rrr.transform.localEulerAngles = RRR.transform.localEulerAngles;
        rrr.transform.GetChild(0).transform.localPosition = RRR.transform.GetChild(0).transform.localPosition;
        rrr.transform.GetChild(0).transform.localEulerAngles = RRR.transform.GetChild(0).transform.localEulerAngles;
        rrr.transform.GetChild(0).GetChild(0).transform.localPosition =
            RRR.transform.GetChild(0).GetChild(0).transform.localPosition;
        rrr.transform.GetChild(0).GetChild(0).transform.localEulerAngles =
            RRR.transform.GetChild(0).GetChild(0).transform.localEulerAngles;
    }
}
