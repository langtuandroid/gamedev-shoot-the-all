using UnityEngine;

public class SAHitWithForce : MonoBehaviour
{
    [SerializeField] private Vector3 thisdirection;
   
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            GetComponent<Rigidbody>().AddForce(thisdirection * 5000 * Time.deltaTime, ForceMode.Force);
        }
    }
}
