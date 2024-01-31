using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class armnames : MonoBehaviour
{
    public GameObject LLL;
    public GameObject RRR;
    public GameObject leftArm;
    public GameObject RightArm;
    void Start()
    {

        leftArm = GetComponent<PlayerController>().leftArm;
        RightArm = GetComponent<PlayerController>().RightArm;
        if (GameManager.Instance)
        {
            LLL = GameManager.Instance.LLL;
            RRR = GameManager.Instance.RRR;
        }
        leftArm.name = LLL.transform.GetChild(0).name;
        RightArm.name = RRR.transform.GetChild(0).name;

    }
}
