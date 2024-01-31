using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wood : MonoBehaviour
{
    public Vector3 thisdirection;
   
    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="Bullet")
        {
            for(int i=0;i<transform.childCount;i++)
            {
                transform.GetChild(i).transform.gameObject.GetComponent<Rigidbody>().isKinematic=false;
                transform.GetChild(i).transform.gameObject.GetComponent<Rigidbody>().
                    AddForce(thisdirection * 5000 * Time.deltaTime);
            }
        }
    }
}
